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
    private readonly GeneratorExecutionContext _context;
    private readonly ViewModelClassToGenerate _classToGenerate;
    private readonly StringBuilder _stringBuilder;
    private readonly string _indent;
    private readonly IEnumerable<CommandInfo> _commandInfos;

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
      foreach(var member in _classToGenerate.ClassDeclarationSyntax.Members)
      {
        if(member is FieldDeclarationSyntax fieldDeclarationSyntax)
        {
         var attributeSyntax = fieldDeclarationSyntax.AttributeLists
            .SelectMany(x => x.Attributes)
           .Where(x => x.Name.ToString() == nameof(PropertyAttribute)
             || x.Name.ToString() == nameof(PropertyAttribute).Replace("Attribute", ""))
           .FirstOrDefault();

          if (attributeSyntax is not null)
          {
            var propertyType = fieldDeclarationSyntax.Declaration.Type.ToString();
            
            bool propertyNameSpecifiedInAttribute = false;
            // TODO: Find out if property name was specified with attribute

            string? propertyName=null;
            var fieldName = fieldDeclarationSyntax.Declaration.Variables.First().Identifier.ToString();

            if (propertyName is null)
            {
              propertyName = fieldName;
              if(propertyName.StartsWith("_"))
              {
                propertyName = propertyName.Substring(1);
              }
              else if (propertyName.StartsWith("m_"))
              {
                propertyName = propertyName.Substring(2);
              }

              var firstCharacter = propertyName.Substring(0, 1).ToUpper();
              
              propertyName = propertyName.Length>1 
                ? firstCharacter + propertyName.Substring(1)
                : firstCharacter;

            }

            if(propertyName is not null)
            {
              GenerateProperty(propertyType, propertyName, fieldName);
            }
          }
        }
      }
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
        _stringBuilder.AppendLine();
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
                GenerateProperty(propertySymbol.Type.ToString(),
                  propertySymbol.Name,$"Model.{propertySymbol.Name}");
              }
            }
          }
        }
      }
    }

    private void GenerateProperty(string propertyType, string propertyName, string backingFieldName)
    {
      var commandsToInvalidate =
         _commandInfos.Where(x => x.CanExecuteAffectingProperties is not null
         && x.CanExecuteAffectingProperties.Contains(propertyName))
         .ToList();
      _stringBuilder.AppendLine(_indent + $"public {propertyType} {propertyName}");
      _stringBuilder.AppendLine(_indent + $"{{");
      _stringBuilder.AppendLine(_indent + $"  get => {backingFieldName};");
      _stringBuilder.AppendLine(_indent + $"  set");
      _stringBuilder.AppendLine(_indent + $"  {{");
      _stringBuilder.AppendLine(_indent + $"    if ({backingFieldName} != value)");
      _stringBuilder.AppendLine(_indent + $"    {{");
      _stringBuilder.AppendLine(_indent + $"      {backingFieldName} = value;");
      //stringBuilder.AppendLine(indent() + $"      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs({propertySymbol.Name}));"); 
      foreach (var commandToInvalidate in commandsToInvalidate)
      {
        _stringBuilder.AppendLine(_indent + $"      {commandToInvalidate.PropertyName}.RaiseCanExecuteChanged();");
      }

      _stringBuilder.AppendLine(_indent + $"    }}");
      _stringBuilder.AppendLine(_indent + $"  }}");
      _stringBuilder.AppendLine(_indent + $"}}");
    }
  }
}