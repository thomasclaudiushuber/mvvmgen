// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;

namespace MvvmGen.Inspectors
{
    internal class ViewModelBaseClassInspector
    {
        internal static bool Inspect(INamedTypeSymbol viewModelClassSymbol, INamedTypeSymbol viewModelBaseClassSymbol)
        {
            return !ViewModelInheritsAlreadyFromViewModelBase(viewModelClassSymbol, viewModelBaseClassSymbol);
        }

        private static bool ViewModelInheritsAlreadyFromViewModelBase(INamedTypeSymbol viewModelClassSymbol,
                INamedTypeSymbol viewModelBaseClassSymbol)
        {
            var inherits = false;

            var currentBaseType = viewModelClassSymbol.BaseType;

            while (currentBaseType is not null)
            {
                if (currentBaseType.Equals(viewModelBaseClassSymbol, SymbolEqualityComparer.Default)
                    || currentBaseType.GetAttributes().Any(x => x.AttributeClass?.ToDisplayString() == "MvvmGen.ViewModelAttribute"))
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
