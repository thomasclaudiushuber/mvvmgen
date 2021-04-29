using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace MvvmGen.Generator
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
        NamespaceDeclarationSyntax? namespaceDeclarationSyntax =
          classToGenerate.ClassDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

        int indentLevel = 0;
        int indentSpaces = 2;
        string indent()
        {
          return new string(' ', indentLevel * indentSpaces);
        }

        var stringBuilder = new StringBuilder();

        // Add using directives
        stringBuilder.AppendLine("using MvvmGen.Core;");

        // Add namespace declaration
        if (namespaceDeclarationSyntax is not null)
        {
          stringBuilder.AppendLine($"namespace {namespaceDeclarationSyntax.Name}");
          stringBuilder.AppendLine("{");
          indentLevel++;
        }

        // Generate class declaration
        stringBuilder.Append(indent());
        stringBuilder.AppendLine($"public partial class {classToGenerate.ClassDeclarationSyntax.Identifier}");
        stringBuilder.Append(indent());
        stringBuilder.AppendLine("{");
        indentLevel++;

        // Generate commands
        var commandGenerator = new CommandGenerator(classToGenerate, stringBuilder, indent());
        var commandInfos = commandGenerator.Generate();

        // Generate properties
        var propertyGenerator = new PropertyGenerator(context, classToGenerate, stringBuilder, indent(), commandInfos);
        propertyGenerator.Generate();

        while (indentLevel > 0)
        {
          indentLevel--;
          stringBuilder.Append(indent());
          stringBuilder.AppendLine("}");
        }

        var sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);
        context.AddSource($"{classToGenerate.ClassDeclarationSyntax.Identifier}.generated.cs", sourceText);
      }

      Debug.WriteLine("Execute");
    }

   

    public void Initialize(GeneratorInitializationContext context)
    {
      if (!Debugger.IsAttached)
      {
        Debugger.Launch();
      }

      Debug.WriteLine("Initialize");

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
        var viewModelGeneratorAttribute = classDeclarationSyntax.AttributeLists
          .SelectMany(x => x.Attributes)
          .Where(x => x.Name.ToString() == nameof(ViewModelAttribute)
            || x.Name.ToString() == nameof(ViewModelAttribute).Replace("Attribute", ""))
          .FirstOrDefault();
        
        if (viewModelGeneratorAttribute is not null)
        {
          ClassesToGenerate.Add(new ViewModelClassToGenerate(classDeclarationSyntax,
            viewModelGeneratorAttribute));
        }
      }
    }
  }

  public class ViewModelClassToGenerate
  {
    public ViewModelClassToGenerate(
      ClassDeclarationSyntax classDeclarationSyntax,
      AttributeSyntax viewModelGeneratorAttribute)
    {
      ClassDeclarationSyntax = classDeclarationSyntax;
      ViewModelGeneratorAttributeSyntax = viewModelGeneratorAttribute;
    }

    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

    public AttributeSyntax ViewModelGeneratorAttributeSyntax { get; }

    public TypeOfExpressionSyntax? ModelTypeExpressionSyntax 
      => (TypeOfExpressionSyntax?)
      ViewModelGeneratorAttributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
  }
}