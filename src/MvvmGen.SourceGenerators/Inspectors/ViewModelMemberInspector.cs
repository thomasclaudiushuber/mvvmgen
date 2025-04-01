// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmGen.Extensions;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelMemberInspector
    {
        internal static (List<CommandToGenerate> CommandsToGenerate,
                         IEnumerable<CommandInvalidationToGenerate> CommandInvalidationsToGenerate,
                         List<PropertyToGenerate> PropertiesToGenerate,
                         Dictionary<string, List<string>> PropertyInvalidationsByGeneratedPropertyName)
            Inspect(INamedTypeSymbol viewModelClassSymbol)
        {
            var commandsToGenerate = new List<CommandToGenerate>();
            var propertiesToGenerate = new List<PropertyToGenerate>();
            var commandPropertyInvalidationsByMethodName = new Dictionary<string, List<string>>();
            var commandsToInvalidateByPropertyName = new Dictionary<string, List<string>>();
            var propertyInvalidationsByGeneratedPropertyName = new Dictionary<string, List<string>>();

            var viewModelMembers = viewModelClassSymbol.GetMembers();

            foreach (var memberSymbol in viewModelMembers)
            {
                if (memberSymbol is IMethodSymbol methodSymbol)
                {
                    FindCommandsToGenerate(commandsToGenerate, commandPropertyInvalidationsByMethodName, methodSymbol, viewModelMembers);
                }
                else if (memberSymbol is IFieldSymbol fieldSymbol)
                {
                    FindPropertiesToGenerate(fieldSymbol, propertiesToGenerate);
                }
                else if (memberSymbol is IPropertySymbol propertySymbol)
                {
                    if (propertySymbol.IsPartialDefinition)
                    {
                        FindPropertiesToGenerate(propertySymbol, propertiesToGenerate);
                    }
                    else // Probably a readonly property with a PropertyInvalidate attribute
                    {
                        FindPropertyInvalidations(propertyInvalidationsByGeneratedPropertyName, propertySymbol);
                    }
                }
            }

            foreach (var commandInfo in commandsToGenerate)
            {
                AddCommandToInvalidateToDictionary(commandInfo,
                    commandsToInvalidateByPropertyName,
                    commandPropertyInvalidationsByMethodName);
            }

            var commandInvalidationsToGenerate = commandsToInvalidateByPropertyName.Select(x => new CommandInvalidationToGenerate(x.Key, x.Value));

            return (commandsToGenerate, commandInvalidationsToGenerate, propertiesToGenerate, propertyInvalidationsByGeneratedPropertyName);
        }

        private static void FindPropertyInvalidations(Dictionary<string, List<string>> propertyInvalidationsByPropertyName, IPropertySymbol propertySymbol)
        {
            var attributeDatas = propertySymbol.GetAttributes();
            var invalidateAttributeDatas = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyInvalidateAttribute").ToList();

            foreach (var attr in invalidateAttributeDatas)
            {
                var propertyNameWithAttributes = propertySymbol.Name;

                var attributeSyntax = ((AttributeSyntax?)attr.ApplicationSyntaxReference?.GetSyntax());
                var propertyNames = attributeSyntax?.ArgumentList?.Arguments.SelectMany(x => x.GetStringValues());

                if (propertyNames is not null)
                {
                    foreach (var propertyName in propertyNames)
                    {
                        if (propertyName is { Length: > 0 })
                        {
                            if (!propertyInvalidationsByPropertyName.ContainsKey(propertyName))
                            {
                                propertyInvalidationsByPropertyName[propertyName] = new List<string>();
                            }

                            if (!propertyInvalidationsByPropertyName[propertyName].Contains(propertyNameWithAttributes))
                            {
                                propertyInvalidationsByPropertyName[propertyName].Add(propertyNameWithAttributes);
                            }
                        }
                    }
                }
            }
        }

        private static void FindPropertiesToGenerate(ISymbol symbol, List<PropertyToGenerate> propertiesToGenerate)
        {
            var attributeDatas = symbol.GetAttributes();
            var propertyAttributeData = attributeDatas.FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyAttribute");

            if (propertyAttributeData is not null)
            {
                IFieldSymbol? fieldSymbol = null;
                IPropertySymbol? propertySymbol = null;

                if (symbol.Kind == SymbolKind.Field)
                {
                    fieldSymbol = (IFieldSymbol)symbol;
                }
                else
                {
                    propertySymbol = (IPropertySymbol)symbol;
                }

                string propertyType = symbol.Kind == SymbolKind.Field
                     ? fieldSymbol!.Type.ToString()
                     : propertySymbol!.Type.ToString();

                string? propertyName = propertySymbol?.Name;
                var fieldName = fieldSymbol?.Name;

                if (fieldName is null)
                {
                    fieldName = $"_{propertyName!.Substring(0, 1).ToLower()}{propertyName.Substring(1)}";
                }

                if (propertyName is null)
                {
                    foreach (var arg in propertyAttributeData.ConstructorArguments)
                    {
                        propertyName = arg.Value?.ToString();
                    }

                    foreach (var arg in propertyAttributeData.NamedArguments)
                    {
                        if (arg.Key == "PropertyName")
                        {
                            propertyName = arg.Value.Value?.ToString();
                        }
                    }
                }

                if (propertyName is null)
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

                var eventsToPublish = new List<PropertyEventPublication>();
                var methodsToCall = new List<PropertyMethodCall>();
                var propertyPublishEventAttributes = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyPublishEventAttribute").ToList();
                var propertyCallMethodAttributes = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyCallMethodAttribute").ToList();

                foreach (var propertyPublishEventAttribute in propertyPublishEventAttributes)
                {
                    var eventType = propertyPublishEventAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                    if (eventType is { Length: > 0 })
                    {
                        var eventPublication = new PropertyEventPublication(eventType);

                        foreach (var arg in propertyPublishEventAttribute.NamedArguments)
                        {
                            if (arg.Key == "EventConstructorArgs")
                            {
                                eventPublication.EventConstructorArgs = arg.Value.Value?.ToString();
                            }
                            else if (arg.Key == "EventAggregatorMemberName")
                            {
                                eventPublication.EventAggregatorMemberName = arg.Value.Value?.ToString();
                            }
                            else if (arg.Key == "PublishCondition")
                            {
                                eventPublication.PublishCondition = arg.Value.Value?.ToString();
                            }
                        }

                        eventsToPublish.Add(eventPublication);
                    }
                }

                foreach (var onChangeCallMethodAttribute in propertyCallMethodAttributes)
                {
                    var methodName = onChangeCallMethodAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();

                    if (methodName is { Length: > 0 })
                    {
                        var methodCall = new PropertyMethodCall(methodName);

                        foreach (var arg in onChangeCallMethodAttribute.NamedArguments)
                        {
                            if (arg.Key == "MethodArgs")
                            {
                                methodCall.MethodArgs = arg.Value.Value?.ToString();
                            }
                        }

                        methodsToCall.Add(methodCall);
                    }
                }

                var generateBackingField = symbol.Kind == SymbolKind.Property;

                propertiesToGenerate.Add(new PropertyToGenerate(propertyName, propertyType, fieldName,
                    generateBackingField, isReadOnly: false, accessModifier: (symbol.Kind == SymbolKind.Field ? "public" : propertySymbol!.DeclaredAccessibility.ToString().ToLower()))
                {
                    EventsToPublish = eventsToPublish,
                    MethodsToCall = methodsToCall
                });
            }
        }

        private static void FindCommandsToGenerate(List<CommandToGenerate> commandsToGenerate,
            Dictionary<string,
                List<string>> propertyInvalidations,
            IMethodSymbol methodSymbol,
            System.Collections.Immutable.ImmutableArray<ISymbol> viewModelMembers)
        {
            var methodAttributes = methodSymbol.GetAttributes();
            var commandAttributeData = methodAttributes.FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.CommandAttribute");

            var invalidateAttributeDatas = methodAttributes.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.CommandInvalidateAttribute").ToList();

            if (commandAttributeData is not null)
            {
                var executeMethodInfo = new CommandMethod(methodSymbol.Name)
                {
                    HasParameter = methodSymbol.Parameters.Any(),
                    IsAwaitable = methodSymbol.IsAsync && methodSymbol.ReturnType.Name == "Task"
                };

                var commandPropertyName = $"{methodSymbol.Name}Command";
                var canExecuteMethodName = commandAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();

                foreach (var arg in commandAttributeData.NamedArguments)
                {
                    if (arg.Key == "CanExecuteMethod")
                    {
                        canExecuteMethodName = arg.Value.Value?.ToString();
                    }
                    else if (arg.Key == "PropertyName")
                    {
                        commandPropertyName = arg.Value.Value?.ToString() ?? commandPropertyName;
                    }
                }

                CommandMethod? canExecuteMethodInfo = null;

                if (canExecuteMethodName is not null)
                {
                    var canExecuteMethodSymbol = viewModelMembers.OfType<IMethodSymbol>().FirstOrDefault(x => x.Name == canExecuteMethodName);
                    if (canExecuteMethodSymbol is not null)
                    {
                        canExecuteMethodInfo = new CommandMethod(canExecuteMethodSymbol.Name)
                        {
                            HasParameter = canExecuteMethodSymbol.Parameters.Any(),
                            IsAwaitable = canExecuteMethodSymbol.IsAsync && canExecuteMethodSymbol.ReturnType.Name == "Task"
                        };
                    }
                }

                commandsToGenerate.Add(
               new CommandToGenerate(executeMethodInfo, commandPropertyName)
               {
                   CanExecuteMethod = canExecuteMethodInfo
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

                    // NOTE: The following does not work, as the CommandInvalidateAttribute usually relies on a generated property.
                    //       That generated property is not available as a symbol, as it didn't get compiled yet. And the property can't get compiled yet,
                    //       as the RaiseCanExecuteChanged method of the command might has to be called in the property setter, so that needs to be generated in.
                    //       So, the AttributeData class in the commented foreach loop below will return an empty string for the constructor argument
                    //       of an attribute like [CommandInvalidate(nameof(FirstName))], as the FirstName property didn't get generated yet.
                    //       Solution for this is to use the AttributeSyntax as shown on the next two code lines.
                    //       The attribute syntax contains the pure code as text, including the nameof(FirstName) expression.
                    //       So let's grab the property name there.
                    //
                    //foreach (var arg in attr.ConstructorArguments)
                    //{
                    //  propertyName = arg.Value?.ToString();
                    //}

                    var attributeSyntax = ((AttributeSyntax?)attr.ApplicationSyntaxReference?.GetSyntax());
                    var propertyNames = attributeSyntax?.ArgumentList?.Arguments.SelectMany(x => x.GetStringValues());

                    if (propertyNames is not null)
                    {
                        foreach (var propertyName in propertyNames)
                        {
                            if (propertyName is { Length: > 0 })
                            {
                                if (!propertyInvalidations[methodIdentifier].Contains(propertyName))
                                {
                                    propertyInvalidations[methodIdentifier].Add(propertyName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void AddCommandToInvalidateToDictionary(CommandToGenerate commandInfo,
            Dictionary<string, List<string>> commandsToInvalidateByPropertyName,
            Dictionary<string, List<string>> commandPropertyInvalidationsByMethodName)
        {
            var propertyNames = commandPropertyInvalidationsByMethodName.Where(x =>
             x.Key == commandInfo.ExecuteMethod.Name
             || x.Key == commandInfo.CanExecuteMethod?.Name)
                 .SelectMany(x => x.Value);

            foreach (var propertyName in propertyNames)
            {
                if (!commandsToInvalidateByPropertyName.ContainsKey(propertyName))
                {
                    commandsToInvalidateByPropertyName[propertyName] = new List<string>();
                }
                if (!commandsToInvalidateByPropertyName[propertyName].Contains(commandInfo.PropertyName))
                {
                    commandsToInvalidateByPropertyName[propertyName].Add(commandInfo.PropertyName);
                }
            }
        }
    }
}

