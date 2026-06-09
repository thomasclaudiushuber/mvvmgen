// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that a type is injected into a ViewModel. Generates a constructor parameter and initializes a property with the injected type. Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set.
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

    /// <summary>
    /// Defines the access modifiers for a property.
    /// </summary>
    public enum AccessModifier
    {
        /// <summary>
        /// The property is private.
        /// </summary>
        Private = 1,

        /// <summary>
        /// The property is propected internal.
        /// </summary>
        ProtectedInternal = 2,

        /// <summary>
        /// The property is protected.
        /// </summary>
        Protected = 3,

        /// <summary>
        /// The property is internal.
        /// </summary>
        Internal = 4,

        /// <summary>
        /// The property is public.
        /// </summary>
        Public = 5
    }
}
