// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;
using MvvmGen.SourceGenerators.Extensions;

namespace MvvmGen.Model
{
    internal class CommandInvalidationToGenerate : IEquatable<CommandInvalidationToGenerate?>
    {
        public CommandInvalidationToGenerate(string propertyName, IEnumerable<string> commandNames)
        {
            PropertyName = propertyName;
            CommandNames = commandNames ?? Array.Empty<string>();
        }

        public string PropertyName { get; }

        public IEnumerable<string> CommandNames { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CommandInvalidationToGenerate);
        }

        public bool Equals(CommandInvalidationToGenerate? other)
        {
            return other is not null &&
                   PropertyName == other.PropertyName &&
                   CommandNames.SequenceEqualWithNullCheck(other.CommandNames);
        }

        public override int GetHashCode()
        {
            return 487910435 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
        }
    }
}
