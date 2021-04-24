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

    public string PropertyName { get; set; }
  }
}
