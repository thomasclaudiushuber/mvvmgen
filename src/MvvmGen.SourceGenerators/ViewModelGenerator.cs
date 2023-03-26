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
    public class ViewModelGenerator : IIncrementalGenerator
    {
        private static readonly string _versionString;

        static ViewModelGenerator()
        {
            _versionString = typeof(ViewModelGenerator).Assembly.GetName().Version.ToString(3);
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var viewModelsToGenerate = context.SyntaxProvider.CreateSyntaxProvider(
                 predicate: static (s, _) => IsSyntaxTarget(s),
                 transform: static (s, _) => GetSemanticTarget(s))
                 .Where(static target => target is not null);

            context.RegisterSourceOutput(viewModelsToGenerate,
                static (spc, source) => Execute(spc, source));

#if MVVMGEN_PURECODEGENERATION
            context.RegisterPostInitializationOutput(PureCodeGenerationLibraryLoader.AddLibraryFilesToContext);
#endif
        }

        private static bool IsSyntaxTarget(SyntaxNode s) => s is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };

        private static ViewModelToGenerate? GetSemanticTarget(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
            var viewModelClassSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
            var viewModelBaseClassSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("MvvmGen.ViewModels.ViewModelBase");

            var viewModelAttributeData = viewModelClassSymbol?.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute");

            if (viewModelClassSymbol is null
                || viewModelClassSymbol.ContainingNamespace is null
                || viewModelAttributeData is null
                || viewModelBaseClassSymbol is null)
            {
                return null;
            }

            var (commandsToGenerate,
                commandInvalidationsToGenerate,
                propertiesToGenerate,
                propertyInvalidationsByGeneratedPropertyName) = ViewModelMemberInspector.Inspect(viewModelClassSymbol);

            var accessModifier = viewModelClassSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                _ => ""
            };

            var viewModelToGenerate = new ViewModelToGenerate(
                viewModelClassSymbol.Name,
                viewModelClassSymbol.ContainingNamespace.ToDisplayString())
            {
                ClassAccessModifier = accessModifier,
                InjectionsToGenerate = ViewModelInjectAttributeInspector.Inspect(viewModelClassSymbol),
                GenerateConstructor = ViewModelAttributeInspector.Inspect(viewModelAttributeData),
                ViewModelFactoryToGenerate = ViewModelGenerateFactoryAttributeInspector.Inspect(viewModelClassSymbol),
                InheritFromViewModelBase = ViewModelBaseClassInspector.Inspect(viewModelClassSymbol, viewModelBaseClassSymbol),
                CommandsToGenerate = commandsToGenerate,
                PropertiesToGenerate = propertiesToGenerate,
                CommandInvalidationsToGenerate = commandInvalidationsToGenerate
            };

            viewModelToGenerate.WrappedModelType = ModelMemberInspector.Inspect(viewModelAttributeData, viewModelToGenerate.PropertiesToGenerate);

            viewModelToGenerate.ViewModelInterfaceToGenerate = ViewModelGenerateInterfaceAttributeInspector.Inspect(viewModelClassSymbol, viewModelToGenerate.PropertiesToGenerate);

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

        private static void Execute(SourceProductionContext context, ViewModelToGenerate? viewModelToGenerate)
        {
            if (viewModelToGenerate is null)
            {
                return;
            }

            var fileName = $"{viewModelToGenerate.NamespaceName}.{viewModelToGenerate.ClassName}.g.cs";

            var vmBuilder = new ViewModelBuilder();

            vmBuilder.GenerateCommentHeader(_versionString);

            vmBuilder.GenerateNullableDirective();

            vmBuilder.GenerateUsingDirectives();

            vmBuilder.GenerateNamespace(viewModelToGenerate);

            vmBuilder.GenerateClass(viewModelToGenerate);

            vmBuilder.GenerateConstructor(viewModelToGenerate);

            vmBuilder.GenerateCommandInitializeMethod(viewModelToGenerate.CommandsToGenerate);

            vmBuilder.GenerateCommandProperties(viewModelToGenerate.CommandsToGenerate);

            vmBuilder.GenerateProperties(viewModelToGenerate.PropertiesToGenerate);

            vmBuilder.GenerateModelProperty(viewModelToGenerate.WrappedModelType);

            vmBuilder.GenerateInjectionProperties(viewModelToGenerate.InjectionsToGenerate);

            vmBuilder.GenerateInvalidateCommandsMethod(viewModelToGenerate.CommandInvalidationsToGenerate);

            while (vmBuilder.IndentLevel > 1) // Keep the namespace open for a ViewModel interface and/or a factory class
            {
                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
            }

            vmBuilder.GenerateViewModelInterface(viewModelToGenerate);

            vmBuilder.GenerateFactoryClass(viewModelToGenerate);

            while (vmBuilder.DecreaseIndent())
            {
                vmBuilder.AppendLine("}");
            }

            var sourceText = SourceText.From(vmBuilder.ToString(), Encoding.UTF8);

            context.AddSource(fileName, sourceText);
        }
    }
}
