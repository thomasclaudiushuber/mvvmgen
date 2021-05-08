// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using MvvmGen.SourceGenerators.Generators;
using MvvmGen.SourceGenerators.Inspectors;
using MvvmGen.SourceGenerators.Extensions;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
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

                    if (viewModelToGenerate.GenerateConstructor)
                    {
                        vmBuilder.GenerateConstructor(viewModelToGenerate);
                    }

                    vmBuilder.GenerateCommandInitializeMethod(viewModelToGenerate.CommandsToGenerate);

                    vmBuilder.GenerateCommandProperties(viewModelToGenerate.CommandsToGenerate);

                    vmBuilder.GenerateProperties(viewModelToGenerate.PropertiesToGenerate);

                    vmBuilder.GenerateModelProperty(viewModelToGenerate.WrappedModelType);

                    vmBuilder.GenerateInjectionProperties(viewModelToGenerate.InjectionsToGenerate);

                    while (vmBuilder.IndentLevel > 1)
                    {
                        vmBuilder.DecreaseIndent();
                        vmBuilder.AppendLine("}");
                    }

                    if (viewModelToGenerate.GenerateViewModelFactory)
                    {
                        vmBuilder.GenerateFactoryClass(viewModelToGenerate);
                    }

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

        public int IndentLevel => _indentLevel;

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
                    var viewModelToGenerate = new ViewModelToGenerate(viewModelClassSymbol);

                    viewModelToGenerate.InjectionsToGenerate = ViewModelInjectAttributeInspector.Inspect(viewModelClassSymbol);

                    viewModelToGenerate.GenerateConstructor = true;

                    foreach (var arg in viewModelAttributeData.NamedArguments)
                    {
                        if (arg.Key == "GenerateConstructor")
                        {
                            viewModelToGenerate.GenerateConstructor = (bool?)arg.Value.Value == true;
                        }
                    }

                    var viewModelFactoryAttribute = viewModelClassSymbol.GetAttributes().FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelGenerateFactoryAttribute");
                    viewModelToGenerate.GenerateViewModelFactory = viewModelFactoryAttribute is not null;

                    var (commandsToGenerate, propertiesToGenerate) = ViewModelMemberInspector.Inspect(viewModelClassSymbol);
                    viewModelToGenerate.CommandsToGenerate = commandsToGenerate;
                    viewModelToGenerate.PropertiesToGenerate = propertiesToGenerate;

                    viewModelToGenerate.WrappedModelType = ModelMemberInspector.Inspect(viewModelClassSymbol, viewModelAttributeData, propertiesToGenerate);

                    foreach (var propertyToGenerate in propertiesToGenerate)
                    {
                        propertyToGenerate.CommandsToInvalidate = commandsToGenerate
                          .Where(x => x.CanExecuteAffectingProperties is not null
                           && x.CanExecuteAffectingProperties.Contains(propertyToGenerate.PropertyName))
                         .ToList();
                    }

                    viewModelToGenerate.IsEventSubscriber = viewModelClassSymbol.Interfaces.Any(x => x.ToDisplayString().StartsWith("MvvmGen.Events.IEventSubscriber"));

                    ViewModelsToGenerate.Add(viewModelToGenerate);
                }
            }
        }
    }
}
