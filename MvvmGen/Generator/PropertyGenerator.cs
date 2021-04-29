using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmGen.Generator
{
  internal class PropertyGenerator
  {
    private GeneratorExecutionContext _context;
    private ViewModelClassToGenerate _classToGenerate;
    private StringBuilder _stringBuilder;
    private string _indent;
    private IEnumerable<CommandInfo> _commandInfos;

    public PropertyGenerator(GeneratorExecutionContext context, ViewModelClassToGenerate classToGenerate, StringBuilder stringBuilder, string v, IEnumerable<CommandInfo> commandInfos)
    {
      _context = context;
      _classToGenerate = classToGenerate;
      _stringBuilder = stringBuilder;
      _indent = v;
      _commandInfos = commandInfos;
    }

    internal void Generate()
    {
      GeneratePropertiesForFields();
      GeneratePropertiesForModelProperties();
    }

    private void GeneratePropertiesForFields()
    {
      
    }

    private void GeneratePropertiesForModelProperties()
    {
      if (_classToGenerate.ModelTypeExpressionSyntax is not null)
      {
        TypeSyntax typeSyntax = _classToGenerate.ModelTypeExpressionSyntax.Type;
        var semanticModel = _context.Compilation.GetSemanticModel(_classToGenerate.ClassDeclarationSyntax.SyntaxTree);
        var symbolInfo = semanticModel.GetSymbolInfo(typeSyntax);

        _stringBuilder.Append(_indent);
        _stringBuilder.AppendLine($"public {symbolInfo.Symbol} Model {{ get; set; }}");
        var namedTypeSymbol = symbolInfo.Symbol as INamedTypeSymbol;
        var members = namedTypeSymbol?.GetMembers();
        if (members is not null)
        {
          foreach (var member in members)
          {
            if (member is IMethodSymbol { MethodKind: MethodKind.PropertySet } methodSymbol)
            {
              var propertySymbol = (IPropertySymbol?)methodSymbol.AssociatedSymbol;
              if (propertySymbol is not null)
              {
                var commandsToInvalidate =
                  _commandInfos.Where(x => x.CanExecuteAffectingProperties is not null
                  && x.CanExecuteAffectingProperties.Contains(propertySymbol.Name))
                  .ToList();
                var indention = _indent;
                GenerateProperty(_stringBuilder, indention, propertySymbol.Type.ToString(),
                  propertySymbol.Name,
                  commandsToInvalidate); ;
              }
            }
          }
        }
      }
    }

    private static void GenerateProperty(StringBuilder stringBuilder,
     string indention, string propertyType, string propertyName, List<CommandInfo> commandsToInvalidate)
    {
      stringBuilder.AppendLine(indention + $"public {propertyType} {propertyName}");
      stringBuilder.AppendLine(indention + $"{{");
      stringBuilder.AppendLine(indention + $"  get => Model.{propertyName};");
      stringBuilder.AppendLine(indention + $"  set");
      stringBuilder.AppendLine(indention + $"  {{");
      stringBuilder.AppendLine(indention + $"    if(Model.{propertyName} != value)");
      stringBuilder.AppendLine(indention + $"    {{");
      stringBuilder.AppendLine(indention + $"      Model.{propertyName} = value;");
      //stringBuilder.AppendLine(indent() + $"      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs({propertySymbol.Name}));"); 
      foreach (var commandToInvalidate in commandsToInvalidate)
      {
        stringBuilder.AppendLine(indention + $"      {commandToInvalidate.PropertyName}.RaiseCanExecuteChanged();");
      }

      stringBuilder.AppendLine(indention + $"    }}");
      stringBuilder.AppendLine(indention + $"  }}");
      stringBuilder.AppendLine(indention + $"}}");
    }
  }
}