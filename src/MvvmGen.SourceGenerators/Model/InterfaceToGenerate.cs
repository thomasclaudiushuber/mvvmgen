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
    internal class InterfaceToGenerate : IEquatable<InterfaceToGenerate?>
    {
        public InterfaceToGenerate(string interfaceName)
        {
            InterfaceName = interfaceName;
        }

        public string InterfaceName { get; }

        public IEnumerable<InterfaceProperty>? Properties { get; set; }

        public IEnumerable<InterfaceMethod>? Methods { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as InterfaceToGenerate);
        }

        public bool Equals(InterfaceToGenerate? other)
        {
            return other is not null &&
                   InterfaceName == other.InterfaceName &&
                   Properties.SequenceEqualWithNullCheck(other.Properties) &&
                   Methods.SequenceEqualWithNullCheck(other.Methods);
        }

        public override int GetHashCode()
        {
            return -895056533 + EqualityComparer<string>.Default.GetHashCode(InterfaceName);
        }
    }

    internal class InterfaceProperty : IEquatable<InterfaceProperty?>
    {
        public InterfaceProperty(string propertyName, string propertyType, bool isReadOnly)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            IsReadOnly = isReadOnly;
        }

        public string PropertyName { get; }

        public string PropertyType { get; }

        public bool IsReadOnly { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as InterfaceProperty);
        }

        public bool Equals(InterfaceProperty? other)
        {
            return other is not null &&
                   PropertyName == other.PropertyName &&
                   PropertyType == other.PropertyType &&
                   IsReadOnly == other.IsReadOnly;
        }

        public override int GetHashCode()
        {
            var hashCode = 947620642;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyType);
            hashCode = hashCode * -1521134295 + IsReadOnly.GetHashCode();
            return hashCode;
        }
    }

    internal class InterfaceMethod : IEquatable<InterfaceMethod?>
    {
        public InterfaceMethod(string methodName, string returnType)
        {
            MethodName = methodName;
            ReturnType = returnType;
        }

        public string MethodName { get; }
        public string ReturnType { get; }
        public IEnumerable<InterfaceMethodParameter>? Parameters { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as InterfaceMethod);
        }

        public bool Equals(InterfaceMethod? other)
        {
            return other is not null &&
                   MethodName == other.MethodName &&
                   ReturnType == other.ReturnType &&
                   Parameters.SequenceEqualWithNullCheck(other.Parameters);
        }

        public override int GetHashCode()
        {
            var hashCode = -1927943118;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MethodName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReturnType);
            return hashCode;
        }
    }

    internal class InterfaceMethodParameter : IEquatable<InterfaceMethodParameter?>
    {
        public InterfaceMethodParameter(string parameterName, string parameterType)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
        }

        public string ParameterName { get; }

        public string ParameterType { get; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as InterfaceMethodParameter);
        }

        public bool Equals(InterfaceMethodParameter? other)
        {
            return other is not null &&
                   ParameterName == other.ParameterName &&
                   ParameterType == other.ParameterType;
        }

        public override int GetHashCode()
        {
            var hashCode = 270403491;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ParameterName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ParameterType);
            return hashCode;
        }
    }
}
