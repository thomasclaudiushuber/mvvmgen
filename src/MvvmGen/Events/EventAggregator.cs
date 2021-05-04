// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmGen.Events
{
    public class EventAggregator : IEventAggregator
    {
        internal Dictionary<Type, List<WeakReference>> _eventSubscribers = new();

        public void Publish<TEvent>(TEvent eventToPublish)
        {
            lock (_eventSubscribers)
            {
                if (eventToPublish is null)
                {
                    throw new ArgumentNullException(nameof(eventToPublish));
                }
                if (!_eventSubscribers.ContainsKey(typeof(TEvent)))
                {
                    return;
                }

                var subscribersToRemove = new List<WeakReference>();

                foreach (var subscriber in _eventSubscribers[typeof(TEvent)])
                {
                    if (subscriber.IsAlive)
                    {
                        subscriber.Target.GetType()
                          .GetMethod(nameof(IEventSubscriber<object>.OnEvent), new[] { typeof(TEvent) })
                          .Invoke(subscriber.Target, new object[] { eventToPublish });
                    }
                    else
                    {
                        subscribersToRemove.Add(subscriber);
                    }
                }

                foreach (var subscriber in subscribersToRemove)
                {
                    _eventSubscribers[typeof(TEvent)].Remove(subscriber);
                }
            }
        }

        public void RegisterSubscriber<TSubscriber>(TSubscriber subscriber)
        {
            lock (_eventSubscribers)
            {
                if (subscriber is null)
                {
                    throw new ArgumentNullException(nameof(subscriber));
                }

                var subscriberInterfaces = typeof(TSubscriber).GetInterfaces()
                  .Where(t => t.IsGenericType && t.FullName.StartsWith("MvvmGen.Events.IEventSubscriber")).ToList();

                if (subscriberInterfaces.Any())
                {

                    var weakReference = new WeakReference(subscriber);

                    var eventTypes = subscriberInterfaces.SelectMany(x => x.GenericTypeArguments).Distinct();

                    foreach (var eventType in eventTypes)
                    {
                        if (!_eventSubscribers.ContainsKey(eventType))
                        {
                            _eventSubscribers.Add(eventType, new());
                        }
                        if (!_eventSubscribers[eventType].Any(x => x.IsAlive && x.Target.Equals(subscriber)))
                        {
                            _eventSubscribers[eventType].Add(weakReference);
                        }
                    }
                }
            }
        }
    }
}
