// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelGenerateInterfaceAttributeInspector
    {
        internal static ViewModelInterfaceToGenerate? Inspect(
            INamedTypeSymbol viewModelClassSymbol, IEnumerable<PropertyToGenerate> propertiesToGenerate)
        {
            ViewModelInterfaceToGenerate? viewModelInterfaceToGenerate = null;
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

                var properties = new List<ViewModelInterfaceProperty>();
                var methods = new List<ViewModelInterfaceMethod>();

                foreach (var propertyToGenerate in propertiesToGenerate)
                {
                    properties.Add(new ViewModelInterfaceProperty(propertyToGenerate.PropertyName,
                        propertyToGenerate.PropertyType, propertyToGenerate.IsReadOnly));
                }

                foreach (var memberSymbol in viewModelClassSymbol.GetMembers())
                {
                    if (memberSymbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        if (memberSymbol is IPropertySymbol propertySymbol)
                        {
                            properties.Add(new ViewModelInterfaceProperty(propertySymbol.Name,
                                propertySymbol.Type.ToDisplayString(), propertySymbol.IsReadOnly));
                        }

                        if (memberSymbol is IMethodSymbol methodSymbol
                            && methodSymbol.MethodKind == MethodKind.Ordinary)
                        {
                            var parameters = new List<ViewModelInterfaceMethodParameter>();

                            foreach (var parameterSymbol in methodSymbol.Parameters)
                            {
                                parameters.Add(new ViewModelInterfaceMethodParameter(parameterSymbol.Type.ToDisplayString(), parameterSymbol.Name));
                            }

                            methods.Add(new ViewModelInterfaceMethod(methodSymbol.Name, methodSymbol.ReturnType.ToDisplayString(), parameters));
                        }
                    }
                }

                viewModelInterfaceToGenerate = new ViewModelInterfaceToGenerate(interfaceName, properties, methods);
            }

            return viewModelInterfaceToGenerate;
        }
    }
}
