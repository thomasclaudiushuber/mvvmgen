// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that an <see cref="MvvmGen.ViewModels.IViewModelFactory{T}"/> is generated. Set this attribute on a class that has the <see cref="ViewModelAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ViewModelGenerateFactoryAttribute : Attribute
    {
    }
}
