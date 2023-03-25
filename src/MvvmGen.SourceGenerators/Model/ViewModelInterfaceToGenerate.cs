// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;

namespace MvvmGen.Model
{
    internal class ViewModelInterfaceToGenerate
    {
        public ViewModelInterfaceToGenerate(string interfaceName, IEnumerable<ViewModelInterfaceProperty> properties, List<ViewModelInterfaceMethod> methods)
        {
            InterfaceName = interfaceName;
            Properties = properties;
            Methods = methods;
        }

        public string InterfaceName { get; }

        public IEnumerable<ViewModelInterfaceProperty> Properties { get; }

        public IEnumerable<ViewModelInterfaceMethod> Methods { get; }
    }

    internal class ViewModelInterfaceProperty
    {
        public ViewModelInterfaceProperty(string propertyName, string propertyType, bool isReadOnly)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            IsReadOnly = isReadOnly;
        }

        public string PropertyName { get; }

        public string PropertyType { get; }

        public bool IsReadOnly { get; }
    }

    internal class ViewModelInterfaceMethod
    {
        public ViewModelInterfaceMethod(string methodName, string returnType, IEnumerable<ViewModelInterfaceMethodParameter> parameters)
        {
            MethodName = methodName;
            ReturnType = returnType;
            Parameters = parameters;
        }

        public string MethodName { get; }
        public string ReturnType { get; }
        public IEnumerable<ViewModelInterfaceMethodParameter> Parameters { get; }
    }

    internal class ViewModelInterfaceMethodParameter
    {
        public ViewModelInterfaceMethodParameter(string parameterName, string parameterType)
        {
            ParameterName = parameterName;
            ParameterType = parameterType;
        }

        public string ParameterName { get; }
        public string ParameterType { get; }
    }
}
