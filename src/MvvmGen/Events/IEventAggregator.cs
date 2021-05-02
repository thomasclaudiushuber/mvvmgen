using System;

namespace MvvmGen.Events
{
  public interface IEventAggregator
  {
    void Publish<TEvent>(TEvent eventToPublish);
    void Subscribe<TEvent>(Action<TEvent> eventHandler);
    void Unsubscribe<TEvent>(Action<TEvent> eventHandler);
  }
}