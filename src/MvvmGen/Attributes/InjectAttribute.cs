// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifieds that a type needs to be injected into a ViewModel. Generates a constructor parameter and initializes a property of the specified type. Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectAttribute"/> class.
        /// </summary>
        /// <param name="type">The type that gets injected inby the ViewModel. All properties of the model type will be generated in the ViewModel.</param>
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
