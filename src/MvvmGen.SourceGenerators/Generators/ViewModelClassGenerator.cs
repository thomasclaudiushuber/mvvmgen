// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;

namespace MvvmGen.Generators
{
    internal static class ClassGenerator
    {
        internal static void GenerateClass(this ViewModelBuilder vmBuilder, INamedTypeSymbol viewModelClassSymbol, INamedTypeSymbol viewModelBaseSymbol)
        {
            var inheritFromViewModelBaseClass = !InheritsFromViewModelBase(viewModelClassSymbol, viewModelBaseSymbol);

            vmBuilder.AppendLine($"partial class {viewModelClassSymbol.Name}" + (inheritFromViewModelBaseClass ? " : MvvmGen.ViewModels.ViewModelBase" : ""));
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }

        private static bool InheritsFromViewModelBase(INamedTypeSymbol viewModelClassSymbol, INamedTypeSymbol viewModelBaseSymbol)
        {
            var inherits = false;

            var currentBaseType = viewModelClassSymbol.BaseType;

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

            return inherits;
        }
    }
}
