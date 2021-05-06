// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class OnChangeCallMethodAttribute : Attribute
    {
        public OnChangeCallMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public string? MethodArgs { get; set; }
    }
}
