using System;

namespace MvvmGen.Events
{
  public interface IEventAggregator
  {
    void Publish<T>(T eventArgs);
    void Subscribe<T>(EventHandler<T> myHandler);
    void Unsubscribe<T>(EventHandler<T> myHandler);
  }
}