// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Inspectors
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
                }

                viewModelFactoryToGenerate = new ViewModelFactoryToGenerate(className, interfaceName);
            }

            return viewModelFactoryToGenerate;
        }
    }
}
