﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Inspectors
{
    internal static class ViewModelInjectAttributeInspector
    {
        internal static IEnumerable<InjectionToGenerate> Inspect(INamedTypeSymbol viewModelClassSymbol)
        {
            List<InjectionToGenerate> injectionsToGenerate = new();
            var injectAttributeDatas = viewModelClassSymbol.GetAttributes().Where(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.InjectAttribute").Reverse().ToList();
            foreach (var attr in injectAttributeDatas)
            {
                string? type = attr.ConstructorArguments.FirstOrDefault().Value?.ToString();
                string? propertyName = attr.ConstructorArguments.Skip(1).FirstOrDefault().Value?.ToString();
                int? propertyAccessModifier = (int?)attr.NamedArguments.SingleOrDefault(x => x.Key == "PropertyAccessModifier").Value.Value;

                if (type is not null) // Should never be null
                {
                    if (propertyName is null)
                    {
                        var typeNameWithoutNamespace = type.Split('.').Last();
                        var isInterface = typeNameWithoutNamespace.StartsWith("I") && char.IsUpper(typeNameWithoutNamespace[0]) && char.IsUpper(typeNameWithoutNamespace[1]);
                        propertyName = isInterface 
                            ? typeNameWithoutNamespace.Substring(1) 
                            : typeNameWithoutNamespace;
                    }
                    if (propertyName is not null)
                    {
                        injectionsToGenerate.Add(new InjectionToGenerate(type, propertyName)
                        {
                            PropertyAccessModifier = propertyAccessModifier switch
                            {
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
            }

            return injectionsToGenerate;
        }

    }
}
