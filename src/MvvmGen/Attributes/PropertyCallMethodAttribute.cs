// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that a method should be called in the setter of a generated property. Set this attribute a field that has the <see cref="PropertyAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyCallMethodAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyCallMethodAttribute"/> class.
        /// </summary>
        /// <param name="methodName">The method to call</param>
        public PropertyCallMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }

        /// <summary>
        /// Gets or sets the method to call.
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Gets or sets the method arguments that are passed to the method that is called. As the method is called in the setter of a property, you can specify for example "value" to pass the value of the property as an argument to the method.
        /// </summary>
        public string? MethodArgs { get; set; }
    }
}
