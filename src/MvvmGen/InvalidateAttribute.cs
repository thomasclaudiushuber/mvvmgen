using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public class InvalidateAttribute : Attribute
  {
    public InvalidateAttribute(string propertyName)
    {
      PropertyName = propertyName;
    }

    public string PropertyName { get; }
  }
}
