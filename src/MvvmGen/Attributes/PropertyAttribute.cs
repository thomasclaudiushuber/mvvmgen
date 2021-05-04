// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute() { }

        public PropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets or sets the name of the generated property
        /// </summary>
        public string? PropertyName { get; set; }

        public Type PublishEventOnChange { get; set; }

        public string PublishEventConstructorArgs { get; set; }
    }
}
