// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using Microsoft.CodeAnalysis;
using MvvmGen.SourceGenerators.Extensions;

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class ClassGenerator
    {
        internal static void GenerateClass(this ViewModelBuilder vmBuilder, INamedTypeSymbol viewModelClassSymbol, INamedTypeSymbol viewModelBaseSymbol)
        {
            var inheritFromViewModelBaseClass = !viewModelClassSymbol.InheritsFromViewModelBase(viewModelBaseSymbol);

            vmBuilder.AppendLine($"partial class {viewModelClassSymbol.Name}" + (inheritFromViewModelBaseClass ? " : ViewModelBase" : ""));
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }
    }
}
