// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmGen.Events
{
    /// <summary>
    /// A class to communicate between loosely coupled objects, like for example ViewModels
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        internal Dictionary<Type, List<WeakReference>> _subscribersByEvent = new();

        /// <inheritdoc/>
        public void Publish<TEvent>(TEvent eventToPublish)
        {
            if (eventToPublish is null)
            {
                throw new ArgumentNullException(nameof(eventToPublish));
            }
            lock (_subscribersByEvent)
            {
                if (!_subscribersByEvent.ContainsKey(typeof(TEvent)))
                {
                    return;
                }

                var subscribersToRemove = new List<WeakReference>();

                foreach (var subscriber in _subscribersByEvent[typeof(TEvent)])
                {
                    if (subscriber.IsAlive)
                    {
                        var target = subscriber.Target;
                        if (target is not null)
                        {
                            target.GetType()
                              .GetMethod(nameof(IEventSubscriber<object>.OnEvent), new[] { typeof(TEvent) })
                              ?.Invoke(target, new object[] { eventToPublish });
                        }
                    }
                    else
                    {
                        subscribersToRemove.Add(subscriber);
                    }
                }

                foreach (var subscriber in subscribersToRemove)
                {
                    _subscribersByEvent[typeof(TEvent)].Remove(subscriber);
                }
            }
        }

        /// <inheritdoc/>
        public void RegisterSubscriber<TSubscriber>(TSubscriber subscriber)
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }

            var subscriberInterfaces = typeof(TSubscriber).GetInterfaces()
                .Where(t => t.IsGenericType && t.FullName?.StartsWith("MvvmGen.Events.IEventSubscriber") == true).ToList();
            if (!subscriberInterfaces.Any())
            {
                return;
            }

            var weakReference = new WeakReference(subscriber);

            var eventTypes = subscriberInterfaces.SelectMany(x => x.GenericTypeArguments).Distinct();
            lock (_subscribersByEvent)
            {
                foreach (var eventType in eventTypes)
                {
                    if (!_subscribersByEvent.ContainsKey(eventType))
                    {
                        _subscribersByEvent.Add(eventType, new());
                    }

                    if (!_subscribersByEvent[eventType].Any(x => x.IsAlive && x.Target?.Equals(subscriber) == true))
                    {
                        _subscribersByEvent[eventType].Add(weakReference);
                    }
                }
            }
        }

        /// <summary>
        /// Unregisters an MvvmGen.Events.IEventSubscriber, so that it won't receive events anymore from the IEventAggregator instance.
        /// Note that calling this method is optional for an instance of this <see cref="EventAggregator"/> class, because the
        /// <see cref="EventAggregator"/> stores a subscriber internally in a <see cref="WeakReference"/>, which means
        /// the subscriber can get garbage collected, even if you don't call this UnregisterSubscriber method.
        /// Calling this method though will immediately unregister a subscriber, even before it got garbage collected.
        /// </summary>
        /// <typeparam name="TSubscriber">The subscriber type</typeparam>
        /// <param name="subscriber">The subscriber instance to unregister</param>
        public void UnregisterSubscriber<TSubscriber>(TSubscriber subscriber)
        {
            if (subscriber is null)
            {
                throw new ArgumentNullException(nameof(subscriber));
            }
            lock (_subscribersByEvent)
            {
                foreach (var subscribersByEvent in _subscribersByEvent)
                {
                    var subscribersToRemove = new List<WeakReference>();
                    foreach (var weakReference in subscribersByEvent.Value)
                    {
                        if (!weakReference.IsAlive
                         || weakReference.Target?.Equals(subscriber) == true)
                        {
                            subscribersToRemove.Add(weakReference);
                        }
                    }

                    foreach (var subscriberToRemove in subscribersToRemove)
                    {
                        subscribersByEvent.Value.Remove(subscriberToRemove);
                    }
                }
            }
        }
    }
}
