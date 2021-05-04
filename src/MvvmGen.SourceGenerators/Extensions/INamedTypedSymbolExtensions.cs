// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using Microsoft.CodeAnalysis;

namespace MvvmGen.SourceGenerators.Extensions
{
    public static class INamedTypedSymbolExtensions
    {
        public static bool InheritsFromBaseType(this INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol baseType)
        {
            var inherits = false;
            var currentBaseType = namedTypeSymbol.BaseType;
            while (currentBaseType is not null)
            {
                if (currentBaseType.Equals(baseType, SymbolEqualityComparer.Default))
                {
                    inherits = true;
                    break;
                }
                currentBaseType = currentBaseType.BaseType;
            }
            return inherits;
        }
    }
}
