using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvmGen.Generator
{
  public class CommandInfo
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
  public class CommandGenerator
  {
    internal IEnumerable<CommandInfo> Generate(ViewModelClassToGenerate classToGenerate,
      StringBuilder stringBuilder, string indention)
    {
      var commandsToGenerate = new List<CommandInfo>();
      var propertyInvalidations = new Dictionary<string, List<string>>();
      foreach (var memberDeclarationSyntax in classToGenerate.ClassDeclarationSyntax.Members)
      {
        if (memberDeclarationSyntax is MethodDeclarationSyntax methodDeclarationSyntax)
        {
          var generateCommandAttribute = methodDeclarationSyntax.AttributeLists
       .SelectMany(x => x.Attributes)
       .Where(x => x.Name.ToString() == nameof(CommandAttribute)
         || x.Name.ToString() == nameof(CommandAttribute).Replace("Attribute", ""))
       .FirstOrDefault();

          var invalidateOnPropertyChangeAttributes = methodDeclarationSyntax.AttributeLists
     .SelectMany(x => x.Attributes)
     .Where(x => x.Name.ToString() == nameof(InvalidateOnPropertyChangeAttribute)
       || x.Name.ToString() == nameof(InvalidateOnPropertyChangeAttribute).Replace("Attribute", ""))
     .ToList();

          if (generateCommandAttribute is not null)
          {
            var canExecuteAttributeArgumentSyntax = generateCommandAttribute.ArgumentList?
              .Arguments.FirstOrDefault();

            string? canExecuteMethod = null;
            if (canExecuteAttributeArgumentSyntax is not null)
            {
              canExecuteMethod = canExecuteAttributeArgumentSyntax.Expression.ToString();
              if (canExecuteMethod is not null)
              {
                canExecuteMethod = canExecuteMethod
                  // Extract name from nameof expression
                  .Replace("nameof(", "")
                  .Replace(")", "")
                  // Extract name from string literal
                  .Replace("\"", "");
              }
            }

            commandsToGenerate.Add(
              new CommandInfo(methodDeclarationSyntax.Identifier.ToString())
              {
                CanExecuteMethod = canExecuteMethod
              });
          }

          if (invalidateOnPropertyChangeAttributes.Any())
          {
            foreach (var attr in invalidateOnPropertyChangeAttributes)
            {
              var methodIdentifier = methodDeclarationSyntax.Identifier.ToString();
              if (!propertyInvalidations.ContainsKey(methodIdentifier))
              {
                propertyInvalidations.Add(methodIdentifier, new List<string>());
              }

              var propertyName = attr.ArgumentList?.Arguments.First().ToString();
              
              if (propertyName != null)
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
            }
          }
        }
      }

      void AddPropertyNames(string? methodName, List<string> canExecuteAffectingProperties)
      {
        if(methodName is null)
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

      stringBuilder.AppendLine();
      stringBuilder.AppendLine($"{indention}public void InitializeCommands()");
      stringBuilder.AppendLine($"{indention}{{");
      foreach (var commandInfo in commandsToGenerate)
      {
        stringBuilder.Append($"{indention}  {commandInfo.PropertyName} = new({commandInfo.ExecuteMethod}");
        if (commandInfo.CanExecuteMethod is not null)
        {
          stringBuilder.Append($", {commandInfo.CanExecuteMethod}");
        }
        stringBuilder.AppendLine(");");
      }
      stringBuilder.AppendLine($"{indention}}}");
      foreach (var commandInfo in commandsToGenerate)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine($"{indention}public DelegateCommand {commandInfo.PropertyName} {{ get; private set; }}");
      }

      return commandsToGenerate;
    }
  }
}
