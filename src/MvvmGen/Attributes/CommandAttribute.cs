// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that a DelegateCommand property in the ViewModel should be generated for a method. Set this attribute on methods of a class that has the <see cref="ViewModelAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        public CommandAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute"/> class.
        /// </summary>
        /// <param name="canExecuteMethod">The name of the method with the can-execute logic</param>
        public CommandAttribute(string canExecuteMethod)
        {
            CanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Gets or sets the name of the method with the can-execute logic.
        /// </summary>
        public string? CanExecuteMethod { get; set; }

        /// <summary>
        /// Gets or sets the name of the command property.
        /// </summary>
        public string? CommandName { get; set; }
    }
}
