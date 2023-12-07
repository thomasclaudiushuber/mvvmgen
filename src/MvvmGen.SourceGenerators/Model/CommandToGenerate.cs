// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;

namespace MvvmGen.Model
{
    internal class CommandToGenerate : IEquatable<CommandToGenerate?>
    {
        public CommandToGenerate(CommandMethod executeMethod, string propertyName)
        {
            ExecuteMethod = executeMethod;
            PropertyName = propertyName;
            FieldName = $"_{PropertyName.Substring(0,1).ToLower()}{PropertyName.Substring(1)}";
        }

        public CommandMethod ExecuteMethod { get; }

        public string PropertyName { get; }

        public string FieldName { get; }

        public CommandMethod? CanExecuteMethod { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CommandToGenerate);
        }

        public bool Equals(CommandToGenerate? other)
        {
            return other is not null &&
                   ExecuteMethod.Equals(other.ExecuteMethod) &&
                   PropertyName == other.PropertyName &&
                   EqualityComparer<CommandMethod?>.Default.Equals(CanExecuteMethod, other.CanExecuteMethod);
        }

        public override int GetHashCode()
        {
            var hashCode = -519560379;
            hashCode = hashCode * -1521134295 + ExecuteMethod.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            return hashCode;
        }
    }

    internal class CommandMethod : IEquatable<CommandMethod?>
    {
        public CommandMethod(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool IsAwaitable { get; set; }

        public bool HasParameter { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CommandMethod);
        }

        public bool Equals(CommandMethod? other)
        {
            return other is not null &&
                   Name == other.Name &&
                   IsAwaitable == other.IsAwaitable &&
                   HasParameter == other.HasParameter;
        }

        public override int GetHashCode()
        {
            var hashCode = -1482689867;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + IsAwaitable.GetHashCode();
            hashCode = hashCode * -1521134295 + HasParameter.GetHashCode();
            return hashCode;
        }
    }
}
