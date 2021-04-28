using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
  public class InvalidateOnPropertyChangeAttribute : Attribute
  {
    public InvalidateOnPropertyChangeAttribute(string propertyName)
    {
      PropertyName = propertyName;
    }

    public string PropertyName { get; }
  }
}
