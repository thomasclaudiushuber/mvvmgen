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
    /// Specifies that an interface is generated for your ViewModel class. 
    /// Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set,
    /// and then the generated ViewModel will automatically implement the generated interface
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelGenerateInterfaceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the generated interface.
        /// </summary>
        public string? InterfaceName { get; set; }
    }
}
