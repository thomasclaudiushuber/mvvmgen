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

    public Type? ModelType { get; private set; }
  }
}
