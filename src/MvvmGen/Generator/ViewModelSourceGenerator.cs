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
        stringBuilder.AppendLine("using System.ComponentModel;");
        stringBuilder.AppendLine("using MvvmGen.Core;");
        stringBuilder.AppendLine();
        // TODO: Add also all using directives from source file, so that all types will be found

        // Add namespace declaration
        if (namespaceDeclarationSyntax is null)
        {
          return;
          // TODO: Show an error here. ViewModel class must be top-level within a namespace
        }

        stringBuilder.AppendLine($"namespace {namespaceDeclarationSyntax.Name}");
        stringBuilder.AppendLine("{");
        indentLevel++;

        var generateINotifyPropertyChanged = false;
        INamedTypeSymbol notifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
        generateINotifyPropertyChanged = !classToGenerate.ClassSymbol.Interfaces.Any(x => x == notifySymbol);

        // Generate class declaration
        stringBuilder.Append(indent());
        stringBuilder.AppendLine($"public partial class {classToGenerate.ClassDeclarationSyntax.Identifier}" +
          (generateINotifyPropertyChanged ? " : INotifyPropertyChanged" : ""));
        stringBuilder.Append(indent());
        stringBuilder.AppendLine("{");
        indentLevel++;

        // Generate INotifyPropertyChanged
        if (generateINotifyPropertyChanged)
        {
          stringBuilder.AppendLine($"{indent()}public event PropertyChangedEventHandler PropertyChanged;");
          stringBuilder.AppendLine();
        }

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
        context.AddSource($"{namespaceDeclarationSyntax.Name}.{classToGenerate.ClassDeclarationSyntax.Identifier}.generated.cs", sourceText);
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
            viewModelGeneratorAttribute,
            context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax)));
        }
      }
    }
  }

  public class ViewModelClassToGenerate
  {
    public ViewModelClassToGenerate(
      ClassDeclarationSyntax classDeclarationSyntax,
      AttributeSyntax viewModelGeneratorAttribute,
      INamedTypeSymbol? classSymbol)
    {
      ClassDeclarationSyntax = classDeclarationSyntax;
      ViewModelGeneratorAttributeSyntax = viewModelGeneratorAttribute;
      ClassSymbol = classSymbol;
    }

    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

    public AttributeSyntax ViewModelGeneratorAttributeSyntax { get; }
    public INamedTypeSymbol? ClassSymbol { get; }

    public TypeOfExpressionSyntax? ModelTypeExpressionSyntax
      => (TypeOfExpressionSyntax?)
      ViewModelGeneratorAttributeSyntax.ArgumentList?.Arguments.FirstOrDefault()?.Expression;
  }
}