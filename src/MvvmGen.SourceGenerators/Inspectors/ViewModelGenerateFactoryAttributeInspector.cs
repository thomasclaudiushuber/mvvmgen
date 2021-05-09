// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

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

            if(viewModelFactoryAttribute is not null)
            {
                viewModelFactoryToGenerate = new ViewModelFactoryToGenerate();
            }

            return viewModelFactoryToGenerate;
        }
    }
}
