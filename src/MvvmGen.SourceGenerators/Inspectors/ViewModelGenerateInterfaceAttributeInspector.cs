// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Microsoft.CodeAnalysis;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelGenerateInterfaceAttributeInspector
    {
        internal static InterfaceToGenerate? Inspect(
            INamedTypeSymbol viewModelClassSymbol, IEnumerable<PropertyToGenerate> propertiesToGenerate, IEnumerable<CommandToGenerate> commandsToGenerate)
        {
            InterfaceToGenerate? viewModelInterfaceToGenerate = null;
            var viewModelInterfaceAttribute = viewModelClassSymbol.GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelGenerateInterfaceAttribute");

            if (viewModelInterfaceAttribute is not null)
            {
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

                foreach (var propertyToGenerate in propertiesToGenerate)
                {
                    properties ??= new();
                    properties.Add(new InterfaceProperty(propertyToGenerate.PropertyName,
                        propertyToGenerate.PropertyType, propertyToGenerate.IsReadOnly));
                }

                foreach (var memberSymbol in viewModelClassSymbol.GetMembers())
                {
                    if (memberSymbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        if (memberSymbol is IPropertySymbol propertySymbol)
                        {
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

                            methods ??= new();
                            methods.Add(new InterfaceMethod(methodSymbol.Name, methodSymbol.ReturnType.ToDisplayString()) { Parameters = parameters });
                        }
                    }
                }

                foreach (var commandToGenerate in commandsToGenerate)
                {
                    properties ??= new();
                    properties.Add(new InterfaceProperty(commandToGenerate.PropertyName, "DelegateCommand", true));
                }

                viewModelInterfaceToGenerate = new InterfaceToGenerate(interfaceName) { Properties = properties, Methods = methods };
            }

            return viewModelInterfaceToGenerate;
        }
    }
}
