// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;
using MvvmGen.Events;

namespace MvvmGen
{
    /// <summary>
    /// Specifies that an event should be published in the setter of a generated property. Set this attribute a field that has the <see cref="PropertyAttribute"/> set.
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
        /// Gets or sets the constructor arguments that are passed to the constructor of the <see cref="EventType"/> class. As the event is published in the setter of a property, you can specify for example <code>"value"</code> to pass the value of the property as an argument to the constructor of the event, or you can specify <code>"value?.Id, value?.FirstName"</code> to pass two arguments to the event constructor.
        /// </summary>
        public string? EventConstructorArgs { get; set; }

        /// <summary>
        /// Gets or sets the name of the member that contains the <see cref="IEventAggregator"/> instance. The default value is "EventAggregator".
        /// </summary>
        public string EventAggregatorMemberName { get; set; } = "EventAggregator";

        /// <summary>
        /// Gets or sets a condition that must be met to publish the event. Pass for example in a string like <code>"value is not null"</code>
        /// </summary>
        public string? PublishCondition { get; set; }
    }
}
