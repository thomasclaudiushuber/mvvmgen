// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandInvalidateAttribute : Attribute
    {
        public CommandInvalidateAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}
