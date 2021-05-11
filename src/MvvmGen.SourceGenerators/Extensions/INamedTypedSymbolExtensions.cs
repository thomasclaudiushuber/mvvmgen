// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;

namespace MvvmGen.SourceGenerators.Extensions
{
    public static class INamedTypedSymbolExtensions
    {
        public static bool InheritsFromViewModelBase(this INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol? viewModelBaseSymbol)
        {
            var inherits = false;
            if (viewModelBaseSymbol is not null)
            {
                var currentBaseType = namedTypeSymbol.BaseType;
                while (currentBaseType is not null)
                {
                    if (currentBaseType.Equals(viewModelBaseSymbol, SymbolEqualityComparer.Default)
                        || currentBaseType.GetAttributes().Any(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute"))
                    {
                        inherits = true;
                        break;
                    }
                    currentBaseType = currentBaseType.BaseType;
                }
            }

            return inherits;
        }
    }
}
