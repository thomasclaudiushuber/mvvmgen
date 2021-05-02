using System;
using System.Collections.Generic;

namespace MvvmGen.Events
{
  public class EventAggregator : IEventAggregator
  {
    private readonly Dictionary<Type, List<Delegate>> _handlersByEventType;

    public EventAggregator()
    {
      _handlersByEventType = new Dictionary<Type, List<Delegate>>();
    }

    public void Publish<T>(T eventArgs)
    {
      if (_handlersByEventType.ContainsKey(typeof(T)))
      {
        foreach (var handler in _handlersByEventType[typeof(T)])
        {
          handler.DynamicInvoke(this, eventArgs);
        }
      }
    }

    public void Subscribe<T>(EventHandler<T> eventHandler)
    {
      if (!_handlersByEventType.ContainsKey(typeof(T)))
      {
        _handlersByEventType[typeof(T)] = new List<Delegate>();
      }
      if (!_handlersByEventType[typeof(T)].Contains(eventHandler))
      {
        _handlersByEventType[typeof(T)].Add(eventHandler);
      }
    }

    public void Unsubscribe<T>(EventHandler<T> eventHandler)
    {
      if (_handlersByEventType.ContainsKey(typeof(T))
        && _handlersByEventType[typeof(T)].Contains(eventHandler))
      {
        _handlersByEventType[typeof(T)].Remove(eventHandler);
      }
    }
  }
}
