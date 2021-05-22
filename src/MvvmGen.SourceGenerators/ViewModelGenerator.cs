// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MvvmGen.Generators;
using MvvmGen.Inspectors;
using MvvmGen.Model;

namespace MvvmGen
{
    /// <summary>
    /// Generates ViewModels for classes that are decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    [Generator]
#if MVVMGEN_PURECODEGENERATION
    public class ViewModelAndLibraryGenerator : ISourceGenerator
#else
    public class ViewModelGenerator : ISourceGenerator
#endif
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            {
                return;
            }
            var versionString = GetType().Assembly.GetName().Version.ToString(3);
            var viewModelBaseSymbol = context.Compilation.GetTypeByMetadataName("MvvmGen.ViewModels.ViewModelBase");
            if (viewModelBaseSymbol is not null)
            {
                foreach (var viewModelToGenerate in receiver.ViewModelsToGenerate)
                {
                    var vmBuilder = new ViewModelBuilder();

                    vmBuilder.GenerateCommentHeader(versionString);

                    vmBuilder.GenerateUsingDirectives();

                    vmBuilder.GenerateNamespace(viewModelToGenerate.ViewModelClassSymbol);

                    vmBuilder.GenerateClass(viewModelToGenerate.ViewModelClassSymbol, viewModelBaseSymbol);

                    vmBuilder.GenerateConstructor(viewModelToGenerate);

                    vmBuilder.GenerateCommandInitializeMethod(viewModelToGenerate.CommandsToGenerate);

                    vmBuilder.GenerateCommandProperties(viewModelToGenerate.CommandsToGenerate);

                    vmBuilder.GenerateProperties(viewModelToGenerate.PropertiesToGenerate);

                    vmBuilder.GenerateModelProperty(viewModelToGenerate.WrappedModelType);

                    vmBuilder.GenerateInjectionProperties(viewModelToGenerate.InjectionsToGenerate);

                    while (vmBuilder.IndentLevel > 1) // Keep the namespace open for a factory class
                    {
                        vmBuilder.DecreaseIndent();
                        vmBuilder.AppendLine("}");
                    }

                    vmBuilder.GenerateFactoryClass(viewModelToGenerate);

                    while (vmBuilder.DecreaseIndent())
                    {
                        vmBuilder.AppendLine("}");
                    }

                    var sourceText = SourceText.From(vmBuilder.ToString(), Encoding.UTF8);
                    context.AddSource($"{viewModelToGenerate.ViewModelClassSymbol.ContainingNamespace}.{viewModelToGenerate.ViewModelClassSymbol.Name}.g.cs", sourceText);
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if MVVMGEN_PURECODEGENERATION
            context.RegisterForPostInitialization(PureCodeGenerationLibraryLoader.AddLibraryFilesToContext);
#endif

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Receives all the classes that have the MvvmGen.ViewModelAttribute set.
    /// </summary>
    internal class SyntaxReceiver : ISyntaxContextReceiver
    {
        public List<ViewModelToGenerate> ViewModelsToGenerate { get; } = new();

        /// <summary>
        /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
        /// </summary>
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is ClassDeclarationSyntax
                { AttributeLists: { Count: > 0 } } classDeclarationSyntax)
            {
                var viewModelClassSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
                var viewModelAttributeData = viewModelClassSymbol?.GetAttributes().SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute");

                if (viewModelClassSymbol is not null && viewModelAttributeData is not null)
                {
                    var (commandsToGenerate, propertiesToGenerate) = ViewModelMemberInspector.Inspect(viewModelClassSymbol);

                    var viewModelToGenerate = new ViewModelToGenerate(viewModelClassSymbol)
                    {
                        InjectionsToGenerate = ViewModelInjectAttributeInspector.Inspect(viewModelClassSymbol),
                        GenerateConstructor = ViewModelAttributeInspector.Inspect(viewModelAttributeData),
                        ViewModelFactoryToGenerate = ViewModelGenerateFactoryAttributeInspector.Inspect(viewModelClassSymbol),
                        CommandsToGenerate = commandsToGenerate,
                        PropertiesToGenerate = propertiesToGenerate
                    };

                    viewModelToGenerate.WrappedModelType = ModelMemberInspector.Inspect(viewModelAttributeData, viewModelToGenerate.PropertiesToGenerate);

                    SetCommandsToInvalidatePropertyOnPropertiesToGenerate(viewModelToGenerate.PropertiesToGenerate, viewModelToGenerate.CommandsToGenerate);

                    viewModelToGenerate.IsEventSubscriber = viewModelClassSymbol.Interfaces.Any(x => x.ToDisplayString().StartsWith("MvvmGen.Events.IEventSubscriber"));

                    ViewModelsToGenerate.Add(viewModelToGenerate);
                }
            }
        }

        private static void SetCommandsToInvalidatePropertyOnPropertiesToGenerate(
            IEnumerable<PropertyToGenerate> propertiesToGenerate,
            IEnumerable<CommandToGenerate> commandsToGenerate)
        {
            var commandsWithInvalidationProperties = commandsToGenerate.Where(x => x.CanExecuteAffectingProperties is not null);

            foreach (var propertyToGenerate in propertiesToGenerate)
            {
                propertyToGenerate.CommandsToInvalidate = commandsWithInvalidationProperties
                    .Where(x => x.CanExecuteAffectingProperties.Contains(propertyToGenerate.PropertyName))
                    .ToList();
            }
        }
    }
}
