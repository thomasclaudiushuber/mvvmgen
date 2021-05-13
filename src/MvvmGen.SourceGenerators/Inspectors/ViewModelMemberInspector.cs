// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmGen.SourceGenerators.Extensions;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Inspectors
{
    internal static class ViewModelMemberInspector
    {
        internal static (List<CommandToGenerate> CommandsToGenerate, List<PropertyToGenerate> PropertiesToGenerate)
            Inspect(INamedTypeSymbol viewModelClassSymbol)
        {
            var commandsToGenerate = new List<CommandToGenerate>();
            var propertiesToGenerate = new List<PropertyToGenerate>();
            var propertyInvalidations = new Dictionary<string, List<string>>();
            var viewModelMembers = viewModelClassSymbol.GetMembers();

            foreach (var memberSymbol in viewModelMembers)
            {
                if (memberSymbol is IMethodSymbol methodSymbol)
                {
                    FindCommandsToGenerate(commandsToGenerate, propertyInvalidations, methodSymbol, viewModelMembers);
                }
                else if (memberSymbol is IFieldSymbol fieldSymbol)
                {
                    FindPropertiesToGenerate(fieldSymbol, propertiesToGenerate);
                }
            }

            foreach (var commandInfo in commandsToGenerate)
            {
                var canExecuteAffectingProperties = new List<string>();
                AddPropertyNames(commandInfo.CanExecuteMethod?.Name, canExecuteAffectingProperties, propertyInvalidations);
                AddPropertyNames(commandInfo.ExecuteMethod.Name, canExecuteAffectingProperties, propertyInvalidations);
                commandInfo.CanExecuteAffectingProperties = canExecuteAffectingProperties.ToArray();
            }

            return (commandsToGenerate, propertiesToGenerate);
        }

        private static void FindPropertiesToGenerate(IFieldSymbol fieldSymbol, List<PropertyToGenerate> propertiesToGenerate)
        {
            var attributeDatas = fieldSymbol.GetAttributes();
            var propertyAttributeData = attributeDatas.FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyAttribute");

            if (propertyAttributeData is not null)
            {
                var propertyType = fieldSymbol.Type.ToString();

                string? propertyName = null;
                var fieldName = fieldSymbol.Name;

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

                if (propertyName is not null)
                {
                    var eventsToPublish = new List<EventToPublish>();
                    var methodsToCall = new List<MethodToCall>();
                    var propertyPublishEventAttributes = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyPublishEventAttribute").ToList();
                    var propertyCallMethodAttributes = attributeDatas.Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.PropertyCallMethodAttribute").ToList();

                    foreach (var propertyPublishEventAttribute in propertyPublishEventAttributes)
                    {
                        var eventType = propertyPublishEventAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();
                        if (eventType is { Length: > 0 })
                        {
                            var eventToPublish = new EventToPublish(eventType);

                            foreach (var arg in propertyPublishEventAttribute.NamedArguments)
                            {
                                if (arg.Key == "EventConstructorArgs")
                                {
                                    eventToPublish.EventConstructorArgs = arg.Value.Value?.ToString();
                                }
                                else if (arg.Key == "EventAggregatorMemberName")
                                {
                                    eventToPublish.EventAggregatorMemberName = arg.Value.Value?.ToString();
                                }
                                else if (arg.Key == "PublishCondition")
                                {
                                    eventToPublish.PublishCondition = arg.Value.Value?.ToString();
                                }
                            }

                            eventsToPublish.Add(eventToPublish);
                        }
                    }

                    foreach (var onChangeCallMethodAttribute in propertyCallMethodAttributes)
                    {
                        var methodName = onChangeCallMethodAttribute.ConstructorArguments.FirstOrDefault().Value?.ToString();

                        if (methodName is { Length: > 0 })
                        {
                            var methodToCall = new MethodToCall(methodName);

                            foreach (var arg in onChangeCallMethodAttribute.NamedArguments)
                            {
                                if (arg.Key == "MethodArgs")
                                {
                                    methodToCall.MethodArgs = arg.Value.Value?.ToString();
                                }
                            }

                            methodsToCall.Add(methodToCall);
                        }
                    }

                    propertiesToGenerate.Add(new PropertyToGenerate(propertyName, propertyType, fieldName)
                    {
                        EventsToPublish = eventsToPublish,
                        MethodsToCall = methodsToCall
                    });
                }
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
                var executeMethodInfo = new MethodInfo(methodSymbol.Name)
                {
                    HasParameter = methodSymbol.Parameters.Any(),
                    IsAwaitable = methodSymbol.IsAsync && methodSymbol.ReturnType.Name == "Task"
                };

                var commandName = $"{methodSymbol.Name}Command";
                string? canExecuteMethodName = commandAttributeData.ConstructorArguments.FirstOrDefault().Value?.ToString();

                foreach (var arg in commandAttributeData.NamedArguments)
                {
                    if (arg.Key == "CanExecuteMethod")
                    {
                        canExecuteMethodName = arg.Value.Value?.ToString();
                    }
                    else if (arg.Key == "CommandName")
                    {
                        commandName = arg.Value.Value?.ToString();
                    }
                }

                MethodInfo? canExecuteMethodInfo = null;

                if (canExecuteMethodName is not null)
                {
                    var canExecuteMethodSymbol = viewModelMembers.OfType<IMethodSymbol>().FirstOrDefault(x => x.Name == canExecuteMethodName);
                    if (canExecuteMethodSymbol is not null)
                    {
                        canExecuteMethodInfo = new MethodInfo(canExecuteMethodSymbol.Name)
                        {
                            HasParameter = canExecuteMethodSymbol.Parameters.Any(),
                            IsAwaitable = canExecuteMethodSymbol.IsAsync && canExecuteMethodSymbol.ReturnType.Name == "Task"
                        };
                    }
                }

                commandsToGenerate.Add(
               new CommandToGenerate(executeMethodInfo, commandName)
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

                    string? propertyName = null;

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
                    propertyName = attributeSyntax?.ArgumentList?.Arguments.First().GetStringValueFromAttributeArgument();

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

        private static void AddPropertyNames(string? methodName, List<string> canExecuteAffectingProperties, Dictionary<string, List<string>> propertyInvalidations)
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
    }
}

