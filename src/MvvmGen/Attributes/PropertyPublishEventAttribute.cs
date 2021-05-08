// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that an event should be puslished in the setter of a generated property. Set this attribute a field that has the <see cref="PropertyAttribute"/> set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyPublishEventAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyPublishEventAttribute"/> class.
        /// </summary>
        /// <param name="eventType">The event type to publish</param>
        public PropertyPublishEventAttribute(Type eventType)
        {
            EventType = eventType;
        }

        /// <summary>
        /// Gets or sets the event type to publish
        /// </summary>
        public Type EventType { get; }

        /// <summary>
        /// Gets or sets the constructor arguments that are passed to the constructor of the <see cref="EventType"/> class. As the event is published in the setter of a property, you can specify for example "value" to pass the value of the property as an argument to the constructor of the event.
        /// </summary>
        public string? EventConstructorArgs { get; set; }

        /// <summary>
        /// Gets or sets the name of the member that contains the <see cref="MvvmGen.Events.IEventAggregator"/> instance. The default value is "EventAggregator".
        /// </summary>
        public string EventAggregatorMemberName { get; set; } = "EventAggregator";
    }
}
