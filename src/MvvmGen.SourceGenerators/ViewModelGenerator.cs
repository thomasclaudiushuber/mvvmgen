// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class ViewModelAndLibraryGenerator : IIncrementalGenerator
#else
    public class ViewModelGenerator : IIncrementalGenerator
#endif
    {
        private static readonly string _versionString;

        static ViewModelGenerator()
        {
            _versionString = typeof(ViewModelGenerator).Assembly.GetName().Version.ToString(3);
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var viewModelBaseSymbol = context.CompilationProvider
                .Select(static (c, _) => c.GetTypeByMetadataName("MvvmGen.ViewModels.ViewModelBase"));

            var viewModelsToGenerate = context.SyntaxProvider.CreateSyntaxProvider(
                 predicate: static (s, _) => IsClassWithAttributes(s),
                 transform: static (s, _) => BuildViewModelToGenerate(s))
                 .Where(x => x is not null)
                 .Collect();

            var combined = viewModelBaseSymbol.Combine(viewModelsToGenerate);

            context.RegisterSourceOutput(combined,
                static (spc, source) => Execute(spc, source.Left, source.Right!));

#if MVVMGEN_PURECODEGENERATION
            context.RegisterPostInitializationOutput(PureCodeGenerationLibraryLoader.AddLibraryFilesToContext);
#endif
        }

        private static bool IsClassWithAttributes(SyntaxNode s) => s is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };

        private static ViewModelToGenerate? BuildViewModelToGenerate(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            var viewModelClassSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            var viewModelAttributeData = viewModelClassSymbol?.GetAttributes().First(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute");

            if (viewModelClassSymbol is null || viewModelAttributeData is null)
            {
                return null;
            }

            var (commandsToGenerate,
                commandsToInvalidateByPropertyName,
                propertiesToGenerate,
                propertyInvalidationsByGeneratedPropertyName) = ViewModelMemberInspector.Inspect(viewModelClassSymbol);

            var viewModelToGenerate = new ViewModelToGenerate(viewModelClassSymbol)
            {
                InjectionsToGenerate = ViewModelInjectAttributeInspector.Inspect(viewModelClassSymbol),
                GenerateConstructor = ViewModelAttributeInspector.Inspect(viewModelAttributeData),
                ViewModelFactoryToGenerate = ViewModelGenerateFactoryAttributeInspector.Inspect(viewModelClassSymbol),
                CommandsToGenerate = commandsToGenerate,
                PropertiesToGenerate = propertiesToGenerate,
                CommandsToInvalidateByPropertyName = commandsToInvalidateByPropertyName
            };

            viewModelToGenerate.WrappedModelType = ModelMemberInspector.Inspect(viewModelAttributeData, viewModelToGenerate.PropertiesToGenerate);

            SetPropertiesToInvalidatePropertyOnPropertiesToGenerate(viewModelToGenerate.PropertiesToGenerate, propertyInvalidationsByGeneratedPropertyName);

            viewModelToGenerate.IsEventSubscriber = viewModelClassSymbol.Interfaces.Any(x => x.ToDisplayString().StartsWith("MvvmGen.Events.IEventSubscriber"));

            return viewModelToGenerate;

            static void SetPropertiesToInvalidatePropertyOnPropertiesToGenerate(
                IList<PropertyToGenerate> propertiesToGenerate,
                Dictionary<string, List<string>> propertyInvalidationsByGeneratedPropertyName)
            {
                foreach (var propertiesToInvalidate in propertyInvalidationsByGeneratedPropertyName)
                {
                    var propertyToGenerate = propertiesToGenerate.SingleOrDefault(x => x.PropertyName == propertiesToInvalidate.Key);
                    if (propertyToGenerate is not null)
                    {
                        propertyToGenerate.PropertiesToInvalidate = propertiesToInvalidate.Value;
                    }
                }
            }
        }

        private static void Execute(SourceProductionContext spc,
            INamedTypeSymbol? viewModelBaseSymbol,
            ImmutableArray<ViewModelToGenerate> viewModelsToGenerate)
        {
            if (viewModelBaseSymbol is null)
            {
                return;
            }

            foreach (var viewModelToGenerate in viewModelsToGenerate)
            {
                var vmBuilder = new ViewModelBuilder();

                vmBuilder.GenerateCommentHeader(_versionString);

                vmBuilder.GenerateNullableDirective();

                vmBuilder.GenerateUsingDirectives();

                vmBuilder.GenerateNamespace(viewModelToGenerate.ViewModelClassSymbol);

                vmBuilder.GenerateClass(viewModelToGenerate.ViewModelClassSymbol, viewModelBaseSymbol);

                vmBuilder.GenerateConstructor(viewModelToGenerate);

                vmBuilder.GenerateCommandInitializeMethod(viewModelToGenerate.CommandsToGenerate);

                vmBuilder.GenerateCommandProperties(viewModelToGenerate.CommandsToGenerate);

                vmBuilder.GenerateProperties(viewModelToGenerate.PropertiesToGenerate);

                vmBuilder.GenerateModelProperty(viewModelToGenerate.WrappedModelType);

                vmBuilder.GenerateInjectionProperties(viewModelToGenerate.InjectionsToGenerate);

                vmBuilder.GenerateInvalidateCommandsMethod(viewModelToGenerate.CommandsToInvalidateByPropertyName);

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
                spc.AddSource($"{viewModelToGenerate.ViewModelClassSymbol.ContainingNamespace}.{viewModelToGenerate.ViewModelClassSymbol.Name}.g.cs", sourceText);
            }
        }
    }
}
