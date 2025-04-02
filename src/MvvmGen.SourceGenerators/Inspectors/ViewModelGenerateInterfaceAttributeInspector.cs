// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelGenerateInterfaceAttributeInspector
    {
        internal static InterfaceToGenerate? Inspect(
            INamedTypeSymbol viewModelClassSymbol,
            IEnumerable<PropertyToGenerate>? propertiesToGenerate,
            IEnumerable<CommandToGenerate>? commandsToGenerate,
            SyntaxTree syntaxTree)
        {
            var viewModelInterfaceAttribute = viewModelClassSymbol.GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelGenerateInterfaceAttribute");

            if (viewModelInterfaceAttribute is null)
            {
                return null;
            }

            var interfaceName = $"I{viewModelClassSymbol.Name}";

            foreach (var arg in viewModelInterfaceAttribute.NamedArguments)
            {
                if (arg.Key == "InterfaceName")
                {
                    interfaceName = arg.Value.Value?.ToString() ?? interfaceName;
                }
            }

            List<InterfaceProperty>? properties = null;
            List<InterfaceMethod>? methods = null;

            if (propertiesToGenerate is not null)
            {
                foreach (var propertyToGenerate in propertiesToGenerate)
                {
                    properties ??= new();
                    properties.Add(new InterfaceProperty(propertyToGenerate.PropertyName,
                        propertyToGenerate.PropertyType, propertyToGenerate.IsReadOnly));
                }
            }

            foreach (var memberSymbol in viewModelClassSymbol.GetMembers())
            {
                if (memberSymbol.DeclaredAccessibility == Accessibility.Public)
                {
                    if (memberSymbol is IPropertySymbol propertySymbol)
                    {
                        if (propertySymbol.IsPartialDefinition)
                        {
                            continue; // Partial properties are already part of propertiesToGenerate list and so added already to the properties list
                        }

                        properties ??= new();
                        properties.Add(new InterfaceProperty(propertySymbol.Name,
                            propertySymbol.Type.ToDisplayString(), propertySymbol.IsReadOnly));
                    }

                    if (memberSymbol is IMethodSymbol methodSymbol
                        && methodSymbol.MethodKind == MethodKind.Ordinary)
                    {
                        List<InterfaceMethodParameter>? parameters = null;

                        foreach (var parameterSymbol in methodSymbol.Parameters)
                        {
                            parameters ??= new();
                            parameters.Add(new InterfaceMethodParameter(parameterSymbol.Type.ToDisplayString(), parameterSymbol.Name));
                        }

                        string[]? genericTypeParamters = null;
                        string[]? genericTypeConstraints = null;

                        if (methodSymbol.IsGenericMethod)
                        {
                            genericTypeParamters = new string[methodSymbol.TypeParameters.Length];
                            for (int i = 0; i < methodSymbol.TypeParameters.Length; i++)
                            {
                                var typeParam = methodSymbol.TypeParameters[i];

                                genericTypeParamters[i] = typeParam.Name;
                            }

                            var methodLocation = methodSymbol.Locations.FirstOrDefault();
                            if (methodLocation is not null)
                            {
                                var methodDeclarationSyntax = syntaxTree.GetRoot().FindNode(methodLocation.SourceSpan) as MethodDeclarationSyntax;
                                if (methodDeclarationSyntax is { ConstraintClauses: { Count: > 0 } })
                                {
                                    genericTypeConstraints =new string[methodDeclarationSyntax.ConstraintClauses.Count];
                                    for (int i = 0; i < methodDeclarationSyntax.ConstraintClauses.Count; i++)
                                    {
                                        var constraintClause = methodDeclarationSyntax.ConstraintClauses[i].ToString();
                                        genericTypeConstraints[i] = constraintClause;
                                    }
                                }
                            }
                        }

                        methods ??= new();
                        methods.Add(
                            new InterfaceMethod(methodSymbol.Name,
                            methodSymbol.ReturnType.ToDisplayString(),
                            methodSymbol.IsGenericMethod)
                            {
                                Parameters = parameters,
                                GenericTypeParameters = genericTypeParamters,
                                GenericTypeConstraints = genericTypeConstraints
                            });
                    }
                }
            }

            if (commandsToGenerate is not null)
            {
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    properties ??= new();
                    properties.Add(new InterfaceProperty(commandToGenerate.PropertyName, "IDelegateCommand", true));
                }
            }

            return new InterfaceToGenerate(interfaceName) { Properties = properties, Methods = methods };
        }
    }
}
