// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace MvvmGen.SourceGenerators.Model
{
    /// <summary>
    /// Contains all the details that must be generated for a class that is decorated with the MvvmGen.ViewModelAttribute.
    /// </summary>
    internal class ViewModelToGenerate
    {
        public ViewModelToGenerate(INamedTypeSymbol viewModelClassSymbol)
        {
            ViewModelClassSymbol = viewModelClassSymbol;
        }

        public INamedTypeSymbol ViewModelClassSymbol { get; }

        public string? WrappedModelType { get; internal set; }

        public bool IsEventSubscriber { get; internal set; }

        public bool GenerateConstructor { get; internal set; }

        public IEnumerable<CommandToGenerate>? CommandsToGenerate { get; set; }

        public IList<PropertyToGenerate>? PropertiesToGenerate { get; internal set; }

        public IEnumerable<InjectionToGenerate>? InjectionsToGenerate { get; internal set; }

        public ViewModelFactoryToGenerate? ViewModelFactoryToGenerate { get; internal set; }
    }
}
