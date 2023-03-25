// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelGenerateFactoryAttributeInspector
    {
        internal static ViewModelFactoryToGenerate? Inspect(INamedTypeSymbol viewModelClassSymbol)
        {
            ViewModelFactoryToGenerate? viewModelFactoryToGenerate = null;
            var viewModelFactoryAttribute = viewModelClassSymbol.GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelGenerateFactoryAttribute");

            if (viewModelFactoryAttribute is not null)
            {
                var className = $"{viewModelClassSymbol.Name}Factory";
                var interfaceName = $"I{className}";
                string? customReturnType = null;

                foreach (var arg in viewModelFactoryAttribute.NamedArguments)
                {
                    if (arg.Key == "ClassName")
                    {
                        className = arg.Value.Value?.ToString() ?? className;
                    }
                    else if (arg.Key == "InterfaceName")
                    {
                        interfaceName = arg.Value.Value?.ToString() ?? interfaceName;
                    }
                    else if (arg.Key == "ReturnType")
                    {
                        customReturnType = arg.Value.Value?.ToString();
                    }
                }

                viewModelFactoryToGenerate = new ViewModelFactoryToGenerate(className, interfaceName, customReturnType);
            }

            return viewModelFactoryToGenerate;
        }
    }
}
