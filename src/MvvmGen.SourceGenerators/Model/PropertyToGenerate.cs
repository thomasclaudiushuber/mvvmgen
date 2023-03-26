// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using System.Collections.Generic;
using MvvmGen.SourceGenerators.Extensions;

namespace MvvmGen.Model
{
    internal class PropertyToGenerate : IEquatable<PropertyToGenerate?>
    {
        public PropertyToGenerate(string propertyName, string propertyType, string backingField, bool isReadOnly = false)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            BackingField = backingField;
            IsReadOnly = isReadOnly;
        }

        public string PropertyName { get; }

        public string PropertyType { get; }

        public string BackingField { get; }

        public bool IsReadOnly { get; }

        public IEnumerable<PropertyEventPublication>? EventsToPublish { get; set; }

        public IEnumerable<PropertyMethodCall>? MethodsToCall { get; set; }

        public IEnumerable<string>? PropertiesToInvalidate { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PropertyToGenerate);
        }

        public bool Equals(PropertyToGenerate? other)
        {
            return other is not null &&
                   PropertyName == other.PropertyName &&
                   PropertyType == other.PropertyType &&
                   BackingField == other.BackingField &&
                   IsReadOnly == other.IsReadOnly &&
                   EventsToPublish.SequenceEqualWithNullCheck(other.EventsToPublish) &&
                   MethodsToCall.SequenceEqualWithNullCheck(other.MethodsToCall) &&
                   PropertiesToInvalidate.SequenceEqualWithNullCheck(other.PropertiesToInvalidate);
        }

        public override int GetHashCode()
        {
            var hashCode = -1061486764;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BackingField);
            hashCode = hashCode * -1521134295 + IsReadOnly.GetHashCode();
            return hashCode;
        }
    }

    internal class PropertyEventPublication : IEquatable<PropertyEventPublication?>
    {
        public PropertyEventPublication(string eventType)
        {
            EventType = eventType;
        }

        public string EventType { get; }

        public string? EventConstructorArgs { get; set; }

        public string? EventAggregatorMemberName { get; set; } = "EventAggregator";

        public string? PublishCondition { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PropertyEventPublication);
        }

        public bool Equals(PropertyEventPublication? other)
        {
            return other is not null &&
                   EventType == other.EventType &&
                   EventConstructorArgs == other.EventConstructorArgs &&
                   EventAggregatorMemberName == other.EventAggregatorMemberName &&
                   PublishCondition == other.PublishCondition;
        }

        public override int GetHashCode()
        {
            var hashCode = 595789827;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(EventType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(EventConstructorArgs);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(EventAggregatorMemberName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(PublishCondition);
            return hashCode;
        }
    }

    internal class PropertyMethodCall : IEquatable<PropertyMethodCall?>
    {
        public PropertyMethodCall(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public string? MethodArgs { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PropertyMethodCall);
        }

        public bool Equals(PropertyMethodCall? other)
        {
            return other is not null &&
                   MethodName == other.MethodName &&
                   MethodArgs == other.MethodArgs;
        }

        public override int GetHashCode()
        {
            var hashCode = -1757553832;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MethodName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(MethodArgs);
            return hashCode;
        }
    }
}
