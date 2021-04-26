using System;

namespace MvvmGen
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
  public class ViewModelGeneratorAttribute : Attribute
  {
    public ViewModelGeneratorAttribute(Type modelType)
    {
      ModelType = modelType;
    }

    public Type ModelType { get; }
  }
}
