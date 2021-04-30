using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace MvvmGen.SourceGenerator
{
  [Generator]
  public class ViewModelSourceGenerator : ISourceGenerator
  {
    public void Execute(GeneratorExecutionContext context)
    {
      if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
      {
        return;
      }

      foreach (var classToGenerate in receiver.ClassesToGenerate)
      {
        int indentLevel = 0;
        int indentSpaces = 2;
        string indent()
        {
          return new string(' ', indentLevel * indentSpaces);
        }

        var classSymbol = classToGenerate.ClassSymbol;
        var namespaceSymbol = classSymbol.ContainingNamespace;

        var stringBuilder = new StringBuilder();

        // Add using directives
        stringBuilder.AppendLine("using System.ComponentModel;");
        stringBuilder.AppendLine("using MvvmGen.Commands;");
        stringBuilder.AppendLine("using MvvmGen.ViewModels;");
        stringBuilder.AppendLine();
        // TODO: Add also all using directives from source file, so that all types will be found

        // Add namespace declaration
        if (namespaceSymbol is null)
        {
          return;
          // TODO: Show an error here. ViewModel class must be top-level within a namespace
        }

        stringBuilder.AppendLine($"namespace {namespaceSymbol}");
        stringBuilder.AppendLine("{");
        indentLevel++;

        var objectSymbol = context.Compilation.GetTypeByMetadataName("System.Object");
        var inheritFromViewModelBase = classSymbol.BaseType.Equals(objectSymbol);

        // Generate class declaration
        stringBuilder.Append(indent());
        stringBuilder.AppendLine($"public partial class {classSymbol.Name}" +
          (inheritFromViewModelBase? " : ViewModelBase" : ""));
        stringBuilder.Append(indent());
        stringBuilder.AppendLine("{");
        indentLevel++;

        // Generate commands
        var commandGenerator = new CommandGenerator(classToGenerate, stringBuilder, indent());
        var commandInfos = commandGenerator.Generate();

        // Generate properties
        var propertyGenerator = new PropertyGenerator(classToGenerate, stringBuilder, indent(), commandInfos);
        propertyGenerator.Generate();

        while (indentLevel > 0)
        {
          indentLevel--;
          stringBuilder.Append(indent());
          stringBuilder.AppendLine("}");
        }

        var sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);
        context.AddSource($"{namespaceSymbol}.{classSymbol.Name}.generated.cs", sourceText);
      }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
      //if(!Debugger.IsAttached)
      //{
      //  Debugger.Launch();
      //}

      context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
  }

  class SyntaxReceiver : ISyntaxContextReceiver
  {
    public List<ViewModelClassToGenerate> ClassesToGenerate { get; } = new();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
      if (context.Node is ClassDeclarationSyntax
        { AttributeLists: { Count: > 0 } } classDeclarationSyntax)
      {
        var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        var viewModelAttributeData = classSymbol?.GetAttributes()
          .SingleOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute");

        if (classSymbol is not null && viewModelAttributeData is not null)
        {
          ClassesToGenerate.Add(new ViewModelClassToGenerate(classDeclarationSyntax,
            classSymbol,
            viewModelAttributeData));
        }
      }
    }
  }

  public class ViewModelClassToGenerate
  {
    public ViewModelClassToGenerate(
      ClassDeclarationSyntax classDeclarationSyntax,
      INamedTypeSymbol classSymbol,
      AttributeData viewModelAttributeData)
    {
      ClassSymbol = classSymbol;
      ViewModelAttributeData = viewModelAttributeData;
    }

    public INamedTypeSymbol ClassSymbol { get; }
    
    public AttributeData ViewModelAttributeData { get; }

    public TypedConstant? ModelTypedConstant => (TypedConstant?) ViewModelAttributeData.ConstructorArguments.FirstOrDefault();
  }
}