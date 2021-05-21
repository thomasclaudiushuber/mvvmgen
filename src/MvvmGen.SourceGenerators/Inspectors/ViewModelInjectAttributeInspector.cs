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
    internal static class ViewModelInjectAttributeInspector
    {
        internal static IEnumerable<InjectionToGenerate> Inspect(INamedTypeSymbol viewModelClassSymbol)
        {
            List<InjectionToGenerate> injectionsToGenerate = new();
            var injectAttributeDatas = viewModelClassSymbol.GetAttributes()
                .Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.InjectAttribute")
                .Reverse()
                .ToList();

            foreach (var attr in injectAttributeDatas)
            {
                var injectedType = attr.ConstructorArguments.FirstOrDefault().Value?.ToString();
                var propertyName = attr.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
                int? propertyAccessModifier = null;

                foreach (var namedArgument in attr.NamedArguments)
                {
                    if (namedArgument.Key == "PropertyAccessModifier")
                    {
                        propertyAccessModifier = (int?)namedArgument.Value.Value;
                    }
                    else if (namedArgument.Key == "PropertyName")
                    {
                        propertyName = namedArgument.Value.Value?.ToString();
                    }
                }

                if (injectedType is not null) // Should never be null
                {
                    if (propertyName is null)
                    {
                        var typeNameWithoutNamespace = injectedType.Split('.').Last();
                        var isInterface = typeNameWithoutNamespace.StartsWith("I") && char.IsUpper(typeNameWithoutNamespace[0]) && char.IsUpper(typeNameWithoutNamespace[1]);
                        propertyName = isInterface
                            ? typeNameWithoutNamespace.Substring(1)
                            : typeNameWithoutNamespace;
                    }

                    injectionsToGenerate.Add(new InjectionToGenerate(injectedType, propertyName) {
                        PropertyAccessModifier = propertyAccessModifier switch {
                            1 => "private",
                            2 => "protected internal",
                            3 => "protected",
                            4 => "internal",
                            5 => "public",
                            _ => "protected"
                        }
                    });
                }
            }

            return injectionsToGenerate;
        }
    }
}

