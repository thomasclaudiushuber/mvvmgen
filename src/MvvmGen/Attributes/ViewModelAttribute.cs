// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class ViewModelAttribute : Attribute
  {
    public ViewModelAttribute() { }

    public ViewModelAttribute(Type modelType)
    {
      ModelType = modelType;
    }

    public Type? ModelType { get; }
  }
}
