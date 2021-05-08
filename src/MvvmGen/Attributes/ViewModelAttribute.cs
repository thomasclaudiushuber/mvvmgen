// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that a class is a ViewModel. With this attribute set on a class, a partial class definition will be generated.
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
        /// Gets or sets if a constructor is generated.
        /// </summary>
        public bool GenerateConstructor { get; set; } = true;
    }
}
