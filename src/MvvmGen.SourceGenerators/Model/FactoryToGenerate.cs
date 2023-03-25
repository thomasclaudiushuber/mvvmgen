// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace MvvmGen.Model
{
    internal class FactoryToGenerate : IEquatable<FactoryToGenerate?>
    {
        public FactoryToGenerate(string className, string interfaceName, string? customReturnType)
        {
            ClassName = className;
            InterfaceName = interfaceName;
            CustomReturnType = customReturnType;
        }

        public string ClassName { get; }

        public string InterfaceName { get; }

        public string? CustomReturnType { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FactoryToGenerate);
        }

        public bool Equals(FactoryToGenerate? other)
        {
            return other is not null &&
                   ClassName == other.ClassName &&
                   InterfaceName == other.InterfaceName &&
                   CustomReturnType == other.CustomReturnType;
        }

        public override int GetHashCode()
        {
            var hashCode = 1967893543;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(InterfaceName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(CustomReturnType);
            return hashCode;
        }
    }
}
