using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class CommandAttribute : Attribute
  {
    public string? CanExecuteMethod { get; set; }
  }
}
