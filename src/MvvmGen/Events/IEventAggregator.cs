// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

namespace MvvmGen.Events
{
    /// <summary>
    /// Provides methods to communicate between loosely coupled objects.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// Publishes an event.
        /// </summary>
        /// <typeparam name="TEvent">The event type to publish</typeparam>
        /// <param name="eventToPublish">The event instance to publish</param>
        void Publish<TEvent>(TEvent eventToPublish);

        /// <summary>
        /// Registers an MvvmGen.Events.IEventSubscriber, so that it will receive events from the IEventAggregator instance.
        /// </summary>
        /// <typeparam name="TSubscriber">The subscriber type</typeparam>
        /// <param name="subscriber">The subscriber instance to register</param>
        void RegisterSubscriber<TSubscriber>(TSubscriber subscriber);

        /// <summary>
        /// Unregisters an MvvmGen.Events.IEventSubscriber, so that it won't receive events anymore from the IEventAggregator instance.
        /// </summary>
        /// <typeparam name="TSubscriber">The subscriber type</typeparam>
        /// <param name="subscriber">The subscriber instance to unregister</param>
        void UnregisterSubscriber<TSubscriber>(TSubscriber subscriber);
    }
}
