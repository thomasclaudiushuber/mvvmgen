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
    /// Specifies that a class is a ViewModel. With this attribute set on a class, a partial class definition is generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        /// </summary>
        public ViewModelAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelAttribute"/> class.
        /// </summary>
        /// <param name="modelType">The type of the model that is wrapped by the ViewModel. All properties of the model type will be generated in the ViewModel.</param>
        public ViewModelAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        /// <summary>
        /// Gets or sets the type of the model that is wrapped by the ViewModel. All properties of the model type will be generated in the ViewModel.
        /// </summary>
        public Type? ModelType { get; set; }

        /// <summary>
        /// Gets or sets the name of generated property that contains the wrapped ModelType. If not set, the property has the name Model.
        /// </summary>
        public string? ModelPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the command factory type that is used in the ViewModel. 
        /// If not set, MvvmGen's <see cref="Commands.DelegateCommandFactory"/> will be used.
        /// Note that the specified command factory type must implement the <see cref="Commands.IDelegateCommandFactory"/> interface.
        /// </summary>
        public Type? CommandFactoryType { get; set; }

        /// <summary>
        /// Gets or sets if a constructor is generated. Default value is true.
        /// </summary>
        public bool GenerateConstructor { get; set; } = true;
    }
}
