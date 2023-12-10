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
        /// Gets or sets the name of generated property that contains the wrapped ModelType. If not set, the property has the name Model
        /// </summary>
        public string? ModelPropertyName { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list of model properties that should not be generated
        /// in the ViewModel for the model that you specified with the <see cref="ModelType"/> property.
        /// </summary>
        public string? ModelPropertiesToIgnore { get; set; }

        /// <summary>
        /// Gets or sets if a constructor is generated. Default value is true.
        /// </summary>
        public bool GenerateConstructor { get; set; } = true;

        /// <summary>
        /// Gets or sets the <see cref="MvvmGen.Commands.IDelegateCommand"/> implementation to use.
        /// That your <see cref="MvvmGen.Commands.IDelegateCommand"/> implementation works seemlessly 
        /// with MvvmGen, it must have a constructor with the following signature:
        /// <code>
        /// public YourCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        /// {
        /// }  
        /// </code>
        /// If this property is not set, the <see cref="MvvmGen.Commands.DelegateCommand"/> class is used
        /// as an <see cref="MvvmGen.Commands.IDelegateCommand"/> implementation.
        /// </summary>
        public Type? CommandType { get; set; }
    }
}
