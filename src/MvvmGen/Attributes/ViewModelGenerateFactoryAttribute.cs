// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;
using MvvmGen.ViewModels;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that an <see cref="IViewModelFactory{T}"/> is generated, where T is your ViewModel class. Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelGenerateFactoryAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the generated factory class.
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the generated factory interface.
        /// </summary>
        public string? InterfaceName { get; set; }
    }
}
