using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class PropertyAttribute : Attribute
  {
    public string? PropertyName { get; }
  }
}
