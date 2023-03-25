// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class ClassGenerator
    {
        internal static void GenerateClass(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate, INamedTypeSymbol viewModelBaseSymbol)
        {
            var viewModelClassSymbol = viewModelToGenerate.ViewModelClassSymbol;

            var inheritFromViewModelBaseClass = !InheritsFromViewModelBase(viewModelClassSymbol, viewModelBaseSymbol);

            vmBuilder.Append($"partial class {viewModelClassSymbol.Name}");

            if (inheritFromViewModelBaseClass)
            {
                vmBuilder.Append(" : global::MvvmGen.ViewModels.ViewModelBase");
            }

            if (viewModelToGenerate.ViewModelInterfaceToGenerate is not null)
            {
                if (inheritFromViewModelBaseClass)
                {
                    vmBuilder.Append(", ");
                }
                else
                {
                    vmBuilder.Append(" : ");
                }

                vmBuilder.Append(viewModelToGenerate.ViewModelInterfaceToGenerate.InterfaceName);
            }

            vmBuilder.AppendLine();
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
