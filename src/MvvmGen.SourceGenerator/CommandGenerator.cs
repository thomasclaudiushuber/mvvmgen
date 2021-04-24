using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmGen.SourceGenerator
{
  internal class CommandInfo
  {
    public CommandInfo(string executeMethod)
    {
      ExecuteMethod = executeMethod;
    }
    public string ExecuteMethod { get; }
    public string? CanExecuteMethod { get; set; }
    public string PropertyName => $"{ExecuteMethod}Command";
    public string[]? CanExecuteAffectingProperties { get; set; }
  }

  internal class CommandGenerator
  {
    private readonly ViewModelClassToGenerate _classToGenerate;
    private readonly StringBuilder _stringBuilder;
    private readonly string _indent;

    public CommandGenerator(ViewModelClassToGenerate classToGenerate,
      StringBuilder stringBuilder,
      string indent)
    {
      _classToGenerate = classToGenerate;
      _stringBuilder = stringBuilder;
      _indent = indent;
    }

    internal IEnumerable<CommandInfo> Generate()
    {
      var commandsToGenerate = new List<CommandInfo>();
      var propertyInvalidations = new Dictionary<string, List<string>>();
      foreach (var memberSymbol in _classToGenerate.ClassSymbol.GetMembers())
      {
        if (memberSymbol is IMethodSymbol methodSymbol)
        {
          var commandAttributeData = methodSymbol.GetAttributes()
            .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.CommandAttribute");

            var invalidateAttributeDatas = methodSymbol.GetAttributes()
            .Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.CommandInvalidateAttribute")
            .ToList();

          if (commandAttributeData is not null)
          {
            // NOTE: typedConstant can't handle nameof keyword. AttributeSyntax on the other side can do it

            //var typedConstant = commandAttributeData.ConstructorArguments.FirstOrDefault();

            //string? canExecuteMethod = null;
            //if (typedConstant.Value is not null)
            //{
            //  canExecuteMethod = typedConstant.Value.ToString();
            //}

            var commandAttributeSyntax = ((AttributeSyntax?)commandAttributeData.ApplicationSyntaxReference?.GetSyntax());
            var canExecuteMethod = commandAttributeSyntax?.ArgumentList?.Arguments.FirstOrDefault()?.ToString();

            if (canExecuteMethod is not null)
            {
              canExecuteMethod = canExecuteMethod
                // Extract name from nameof expression
                .Replace("nameof(", "")
                .Replace(")", "")
                // Extract name from string literal
                .Replace("\"", "");
            }
            commandsToGenerate.Add(
               new CommandInfo(methodSymbol.Name)
               {
                 CanExecuteMethod = canExecuteMethod
               });
          }

          if (invalidateAttributeDatas.Any())
          {
            foreach (var attr in invalidateAttributeDatas)
            {
              var methodIdentifier = methodSymbol.Name;
              if (!propertyInvalidations.ContainsKey(methodIdentifier))
              {
                propertyInvalidations.Add(methodIdentifier, new List<string>());
              }

              var attributeSyntax = ((AttributeSyntax?)attr.ApplicationSyntaxReference?.GetSyntax());
              var propertyName = attributeSyntax?.ArgumentList?.Arguments.First().ToString();

              if (propertyName is not null)
              {
                propertyName = propertyName
                // Extract name from nameof expression
                  .Replace("nameof(", "")
                  .Replace(")", "")
                  // Extract name from string literal
                  .Replace("\"", "");

                if (!propertyInvalidations[methodIdentifier].Contains(propertyName))
                {
                  propertyInvalidations[methodIdentifier].Add(propertyName);
                }
              }

              // NOTE: The following code couldn't handle the nameof expression and returned an empty string for that expression.
              //       Seems the TypedConstant of the SemanticModel has issues here, as it is resolved later, and AttributeSyntax can handle it.

              //var propertyNameTypedConstant = attr.ConstructorArguments.FirstOrDefault();

              //if (propertyNameTypedConstant.Value is not null)
              //{
              //  var propertyName = propertyNameTypedConstant.Value.ToString();

              //  if (!propertyInvalidations[methodIdentifier].Contains(propertyName))
              //  {
              //    propertyInvalidations[methodIdentifier].Add(propertyName);
              //  }
              //}
            }
          }
        }
      }

      void AddPropertyNames(string? methodName, List<string> canExecuteAffectingProperties)
      {
        if (methodName is null)
        {
          return;
        }

        if (propertyInvalidations.ContainsKey(methodName))
        {
          foreach (var propertyName in propertyInvalidations[methodName])
          {
            if (!canExecuteAffectingProperties.Contains(propertyName))
            {
              canExecuteAffectingProperties.Add(propertyName);
            }
          }
        }
      }

      foreach (var commandInfo in commandsToGenerate)
      {
        var canExecuteAffectingProperties = new List<string>();
        AddPropertyNames(commandInfo.CanExecuteMethod, canExecuteAffectingProperties);
        AddPropertyNames(commandInfo.ExecuteMethod, canExecuteAffectingProperties);
        commandInfo.CanExecuteAffectingProperties = canExecuteAffectingProperties.ToArray();
      }

      _stringBuilder.AppendLine($"{_indent}public void Initialize()");
      _stringBuilder.AppendLine($"{_indent}{{");
      foreach (var commandInfo in commandsToGenerate)
      {
        _stringBuilder.Append($"{_indent}  {commandInfo.PropertyName} = new({commandInfo.ExecuteMethod}");
        if (commandInfo.CanExecuteMethod is not null)
        {
          _stringBuilder.Append($", {commandInfo.CanExecuteMethod}");
        }
        _stringBuilder.AppendLine(");");
      }
      _stringBuilder.AppendLine($"{_indent}}}");
      foreach (var commandInfo in commandsToGenerate)
      {
        _stringBuilder.AppendLine();
        _stringBuilder.AppendLine($"{_indent}public DelegateCommand {commandInfo.PropertyName} {{ get; private set; }}");
      }

      return commandsToGenerate;
    }
  }
}
