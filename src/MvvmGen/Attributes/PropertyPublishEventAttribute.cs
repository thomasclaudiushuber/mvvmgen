// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;

namespace MvvmGen
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PropertyPublishEventAttribute : Attribute
    {
        public PropertyPublishEventAttribute(Type eventType)
        {
            EventType = eventType;
        }

        public Type EventType { get; }

        public string? EventConstructorArgs { get; set; }

        public string? EventAggregatorMemberName { get; set; }
    }
}
