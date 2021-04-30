using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmGen.SourceGenerator
{
  internal class PropertyGenerator
  {
    private readonly ViewModelClassToGenerate _classToGenerate;
    private readonly StringBuilder _stringBuilder;
    private readonly string _indent;
    private readonly IEnumerable<CommandInfo> _commandInfos;

    public PropertyGenerator(ViewModelClassToGenerate classToGenerate, StringBuilder stringBuilder, string v, IEnumerable<CommandInfo> commandInfos)
    {
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
      foreach (var member in _classToGenerate.ClassSymbol.GetMembers())
      {
        if (member is IFieldSymbol fieldSymbol)
        {
          var attributeData = fieldSymbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyAttribute");

          if (attributeData is not null)
          {
            var propertyType = fieldSymbol.Type.ToString();

            var typedConstant = attributeData.ConstructorArguments.FirstOrDefault();

            string? propertyName = null;
            var fieldName = fieldSymbol.Name;

            if (typedConstant.Value is not null)
            {
              propertyName = typedConstant.Value.ToString();
            }
            else
            {
              propertyName = fieldName;
              if (propertyName.StartsWith("_"))
              {
                propertyName = propertyName.Substring(1);
              }
              else if (propertyName.StartsWith("m_"))
              {
                propertyName = propertyName.Substring(2);
              }

              var firstCharacter = propertyName.Substring(0, 1).ToUpper();

              propertyName = propertyName.Length > 1
                ? firstCharacter + propertyName.Substring(1)
                : firstCharacter;
            }

            if (propertyName is not null)
            {
              GenerateProperty(propertyType, propertyName, fieldName);
            }
          }
        }
      }
    }

    private void GeneratePropertiesForModelProperties()
    {

      if (_classToGenerate.ModelTypedConstant?.Value is not null)
      {
        var model = _classToGenerate.ModelTypedConstant.Value.Value as INamedTypeSymbol;

        if (model is null) return;

        _stringBuilder.Append(_indent);
        _stringBuilder.AppendLine($"public {model} Model {{ get; set; }}");
        _stringBuilder.AppendLine();
        var members = model.GetMembers();
        foreach (var member in members)
        {
          if (member is IMethodSymbol { MethodKind: MethodKind.PropertySet } methodSymbol)
          {
            var propertySymbol = (IPropertySymbol?)methodSymbol.AssociatedSymbol;
            if (propertySymbol is not null)
            {
              GenerateProperty(propertySymbol.Type.ToString(),
                propertySymbol.Name, $"Model.{propertySymbol.Name}");
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
      _stringBuilder.AppendLine(_indent + $"      OnPropertyChanged(\"{propertyName}\");");
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