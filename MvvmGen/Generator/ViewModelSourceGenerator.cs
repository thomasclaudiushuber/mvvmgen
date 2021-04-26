using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

      foreach (var classDeclarationSyntax in receiver.ClassesToGenerate)
      {
        NamespaceDeclarationSyntax? namespaceDeclarationSyntax =
          classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;

        int indentLevel = 0;
        int indentSpaces = 2;
        string indent()
        {
          return new string(' ', indentLevel * indentSpaces);
        }

        var stringBuilder = new StringBuilder();

        if (namespaceDeclarationSyntax is not null)
        {
          stringBuilder.AppendLine($"namespace {namespaceDeclarationSyntax.Name}");
          stringBuilder.AppendLine("{");
          indentLevel++;
        }

        stringBuilder.Append(indent());
        stringBuilder.AppendLine($"public partial class {classDeclarationSyntax.Identifier}");
        stringBuilder.Append(indent());
        stringBuilder.AppendLine("{");
        indentLevel++;

        stringBuilder.Append(indent());
        stringBuilder.AppendLine($@"public string SayHello => ""Hello from generated property"";");

        while (indentLevel > 0)
        {
          indentLevel--;
          stringBuilder.Append(indent());
          stringBuilder.AppendLine("}");
        }

        var sourceText = SourceText.From(stringBuilder.ToString(), Encoding.UTF8);
        context.AddSource($"{classDeclarationSyntax.Identifier}.generated.cs", sourceText);
      }

      Debug.WriteLine("Execute");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
      //if (!Debugger.IsAttached)
      //{
      //  Debugger.Launch();
      //}

      Debug.WriteLine("Initialize");

      context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
    }
  }

  class SyntaxReceiver : ISyntaxContextReceiver
  {
    public List<ClassDeclarationSyntax> ClassesToGenerate { get; } = new();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
      if (context.Node is ClassDeclarationSyntax
        { AttributeLists: { Count: > 0 } } classDeclarationSyntax)
      {
        ClassesToGenerate.Add(classDeclarationSyntax);
      }
    }
  }
}
