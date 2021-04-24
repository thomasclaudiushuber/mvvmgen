using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class PropertyAttribute : Attribute
  {
    public PropertyAttribute() { }

    public PropertyAttribute(string propertyName)
    {
      PropertyName = propertyName;
    }

    public string? PropertyName { get; }
  }
}
