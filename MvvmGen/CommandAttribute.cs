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

    public string? CanExecuteMethod { get; private set; }
  }
}
