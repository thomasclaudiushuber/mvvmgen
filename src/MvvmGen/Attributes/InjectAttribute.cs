// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(Type type)
        {
            Type = type;
        }

        public InjectAttribute(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;

        }

        public Type Type { get; }

        public string? PropertyName { get; }

        public AccessModifier PropertyAccessModifier { get; set; }
    }

    public enum AccessModifier
    {
        Private = 1,
        ProtectedInternal = 2,
        Protected = 3,
        Internal = 4,
        Public = 5
    }
}
