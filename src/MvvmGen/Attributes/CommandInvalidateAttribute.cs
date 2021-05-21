// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;
using System.Linq;
using MvvmGen.Commands;

namespace MvvmGen
{
    /// <summary>
    /// Specifies a property, in which the <see cref="DelegateCommand.RaiseCanExecuteChanged"/> method of a DelegateCommand is called to refresh controls in the UI that are using the <see cref="DelegateCommand"/>. Set this one or more instances of this attribute on the execute or can-execute method of a command that you have defined with the <see cref="CommandAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandInvalidateAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandInvalidateAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property in which the DelegateCommand's <see cref="DelegateCommand.RaiseCanExecuteChanged"/> method is called</param>
        /// <param name="morePropertyNames">More properties in which the DelegateCommand's <see cref="DelegateCommand.RaiseCanExecuteChanged"/> method is called</param>
        public CommandInvalidateAttribute(string propertyName, params string[] morePropertyNames)
        {
            PropertyNames = new[] { propertyName }.Concat(morePropertyNames).ToArray();
        }

        /// <summary>
        /// Gets the property names in which the DelegateCommand's <see cref="DelegateCommand.RaiseCanExecuteChanged"/> method is called.
        /// </summary>
        public string[] PropertyNames { get; }
    }
}
