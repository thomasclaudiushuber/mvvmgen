// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace MvvmGen.Model
{
    internal class InjectionToGenerate : IEquatable<InjectionToGenerate?>
    {
        public InjectionToGenerate(string type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
        }

        public string Type { get; }

        public string PropertyName { get; internal set; }

        public string PropertyAccessModifier { get; set; } = "protected";

        public string SetterAccessModifier => PropertyAccessModifier == "private" ? "" : "private";

        public override bool Equals(object? obj)
        {
            return Equals(obj as InjectionToGenerate);
        }

        public bool Equals(InjectionToGenerate? other)
        {
            return other is not null &&
                   Type == other.Type &&
                   PropertyName == other.PropertyName &&
                   PropertyAccessModifier == other.PropertyAccessModifier &&
                   SetterAccessModifier == other.SetterAccessModifier;
        }

        public override int GetHashCode()
        {
            var hashCode = 1180993528;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyAccessModifier);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SetterAccessModifier);
            return hashCode;
        }
    }
}

