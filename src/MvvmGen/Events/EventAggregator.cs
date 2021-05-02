using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MvvmGen.Events
{
  public class EventAggregator : IEventAggregator
  {
    private readonly ConcurrentDictionary<Type, List<Delegate>> _handlersByEventType = new();

    public void Publish<TEvent>(TEvent eventToPublish)
    {
      if (_handlersByEventType.ContainsKey(typeof(TEvent)))
      {
        foreach (var handler in _handlersByEventType[typeof(TEvent)])
        {
          handler.DynamicInvoke(eventToPublish);
        }
      }
    }

    public void Subscribe<TEvent>(Action<TEvent> eventHandler)
    {
      if (!_handlersByEventType.ContainsKey(typeof(TEvent)))
      {
        _handlersByEventType[typeof(TEvent)] = new List<Delegate>();
      }
      if (!_handlersByEventType[typeof(TEvent)].Contains(eventHandler))
      {
        _handlersByEventType[typeof(TEvent)].Add(eventHandler);
      }
    }

    public void Unsubscribe<TEvent>(Action<TEvent> eventHandler)
    {
      if (_handlersByEventType.ContainsKey(typeof(TEvent))
        && _handlersByEventType[typeof(TEvent)].Contains(eventHandler))
      {
        _handlersByEventType[typeof(TEvent)].Remove(eventHandler);
      }
    }
  }
}
