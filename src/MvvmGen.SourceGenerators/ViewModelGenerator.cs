// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MvvmGen.SourceGenerators.Generators;
using MvvmGen.SourceGenerators.Inspectors;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    /// <summary>
    /// Generates ViewModels for classes that are decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    [Generator]
    public class ViewModelGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
            {
                return;
            }

            var versionString = typeof(ViewModelGenerator).Assembly.GetName().Version.ToString(3);
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
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }

    /// <summary>
    /// Wraps a StringBuilder and manages indention
    /// </summary>
    internal class ViewModelBuilder
    {
        private string _indent = "";
        private const int _indentSpaces = 4;
        private int _indentLevel;
        private bool _wasLastCallApendLine = true;
        private bool _isFirstMember = true;
        private readonly StringBuilder _stringBuilder;

        public ViewModelBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public int IndentLevel => _indentLevel;

        public void IncreaseIndent()
        {
            _indentLevel++;
            _indent += new string(' ', _indentSpaces);
        }

        public bool DecreaseIndent()
        {
            if (_indent.Length >= _indentSpaces)
            {
                _indentLevel--;
                _indent = _indent.Substring(_indentSpaces);
                return true;
            }

            return false;
        }

        public void AppendLineBeforeMember()
        {
            if (!_isFirstMember)
            {
                _stringBuilder.AppendLine();
            }

            _isFirstMember = false;
        }

        public void AppendLine(string line)
        {
            if (_wasLastCallApendLine) // If last call was only Append, you shouldn't add the indent
            {
                _stringBuilder.Append(_indent);
            }

            _stringBuilder.AppendLine($"{line}");
            _wasLastCallApendLine = true;
        }

        public void AppendLine()
        {
            _stringBuilder.AppendLine();
            _wasLastCallApendLine = true;
        }

        public void Append(string stringToAppend)
        {
            if (_wasLastCallApendLine)
            {
                _stringBuilder.Append(_indent);
                _wasLastCallApendLine = false;
            }

            _stringBuilder.Append(stringToAppend);
        }

        public override string ToString() => _stringBuilder.ToString();
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
