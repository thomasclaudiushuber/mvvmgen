using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
      if(!(context.SyntaxContextReceiver is SyntaxReceiver receiver))
      {
        return;
      }

      foreach (var classDeclararationSyntax in receiver.ClassesToGenerate)
      {
        var sourceText = SourceText.From($@"
namespace MvvmGen.WpfApp.ViewModel{{
public partial class {classDeclararationSyntax.Identifier}
{{
  public string SayHello => ""Hello from generated property"";
}}
}}
",Encoding.UTF8);
        context.AddSource($"{classDeclararationSyntax.Identifier}.generated.cs", sourceText);
      }

      Debug.WriteLine("Execute");
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    //  if (!Debugger.IsAttached)
    //  {
    //    Debugger.Launch();
    //  }

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
