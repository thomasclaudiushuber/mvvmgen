// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MvvmGen.SourceGenerators.Model
{
    internal class ViewModelToGenerate
    {
        public ViewModelToGenerate(INamedTypeSymbol viewModelClassSymbol)
        {
            ViewModelClassSymbol = viewModelClassSymbol;
        }

        public IEnumerable<CommandToGenerate>? CommandsToGenerate { get; set; }

        public IEnumerable<PropertyToGenerate>? PropertiesToGenerate { get; internal set; }

        public string? WrappedModelType { get; internal set; }

        public INamedTypeSymbol ViewModelClassSymbol { get; }

        public bool IsEventSubscriber { get; internal set; }

        public bool IsEventAggregatorPropertyRequired => PropertiesToGenerate is not null && PropertiesToGenerate.Any(x => x.EventToPublish is not null);
    }
}
