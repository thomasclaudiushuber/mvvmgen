// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifieds that a type is injected into a ViewModel. Generates a constructor parameter and initializes a property with the injected type. Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InjectAttribute"/> class.
        /// </summary>
        /// <param name="type">The type that is injected into the ViewModel.</param>
        public InjectAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InjectAttribute"/> class.
        /// </summary>
        /// <param name="type">The type that is injected into the ViewModel.</param>
        /// <param name="propertyName">The name of the property that stores the injected type.</param>
        public InjectAttribute(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the type that is injected into the ViewModel.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets or sets the name of the property that stores the injected type.
        /// </summary>
        public string? PropertyName { get; set; }


        /// <summary>
        /// Gets or sets the access modifier of the property that stores the injected type.
        /// </summary>
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
