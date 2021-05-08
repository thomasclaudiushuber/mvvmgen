// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using Microsoft.CodeAnalysis;

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class NamespaceGenerator
    {
        internal static void GenerateNamespace(this ViewModelBuilder vmBuilder, INamedTypeSymbol viewModelClassSymbol)
        {
            vmBuilder.AppendLine();

            // Add namespace declaration
            if (viewModelClassSymbol.ContainingNamespace is null)
            {
                return;
                // TODO: Show an error here. ViewModel class must be top-level within a namespace
            }

            vmBuilder.AppendLine($"namespace {viewModelClassSymbol.ContainingNamespace}");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }
    }
}
