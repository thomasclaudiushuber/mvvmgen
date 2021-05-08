// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.Events
{
    /// <summary>
    /// Subscribes to an event of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent">The event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2> : IEventSubscriber<TEvent1>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent2 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3> : IEventSubscriber<TEvent1, TEvent2>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent3 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4> : IEventSubscriber<TEvent1, TEvent2, TEvent3>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent4 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent5 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    /// <typeparam name="TEvent6">The sixth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent6 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    /// <typeparam name="TEvent6">The sixth event to subscribe</typeparam>
    /// <typeparam name="TEvent7">The seventh event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent7 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    /// <typeparam name="TEvent6">The sixth event to subscribe</typeparam>
    /// <typeparam name="TEvent7">The seventh event to subscribe</typeparam>
    /// <typeparam name="TEvent8">The eighth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent8 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    /// <typeparam name="TEvent6">The sixth event to subscribe</typeparam>
    /// <typeparam name="TEvent7">The seventh event to subscribe</typeparam>
    /// <typeparam name="TEvent8">The eighth event to subscribe</typeparam>
    /// <typeparam name="TEvent9">The ninth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent9 eventData);
    }

    /// <summary>
    /// Subscribes to events of an <see cref="IEventAggregator"/>.
    /// </summary>
    /// <typeparam name="TEvent1">The first event to subscribe</typeparam>
    /// <typeparam name="TEvent2">The second event to subscribe</typeparam>
    /// <typeparam name="TEvent3">The third event to subscribe</typeparam>
    /// <typeparam name="TEvent4">The fourth event to subscribe</typeparam>
    /// <typeparam name="TEvent5">The fifth event to subscribe</typeparam>
    /// <typeparam name="TEvent6">The sixth event to subscribe</typeparam>
    /// <typeparam name="TEvent7">The seventh event to subscribe</typeparam>
    /// <typeparam name="TEvent8">The eighth event to subscribe</typeparam>
    /// <typeparam name="TEvent9">The ninth event to subscribe</typeparam>
    /// <typeparam name="TEvent10">The tenth event to subscribe</typeparam>
    public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9>
    {
        /// <summary>
        /// Gets called when the event occurs
        /// </summary>
        /// <param name="eventData">The event instance</param>
        void OnEvent(TEvent10 eventData);
    }
}
