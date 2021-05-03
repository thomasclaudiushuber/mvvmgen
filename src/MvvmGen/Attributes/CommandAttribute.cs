// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class CommandAttribute : Attribute
  {
    public CommandAttribute() { }

    public CommandAttribute(string canExecuteMethod)
    {
      CanExecuteMethod = canExecuteMethod;
    }

    public string? CanExecuteMethod { get; set; }

    public string? CommandName { get; set; }
  }
}
