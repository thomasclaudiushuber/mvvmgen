// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using Microsoft.CodeAnalysis;

namespace MvvmGen.Inspectors
{
    internal static class ViewModelAttributeInspector
    {
        internal static string? InspectCommandFactoryType(AttributeData viewModelAttributeData)
        {
            string? commandFactoryType = null;

            foreach (var arg in viewModelAttributeData.NamedArguments)
            {
                if (arg.Key == "CommandFactoryType")
                {
                    commandFactoryType = arg.Value.Value?.ToString();
                }
            }

            return commandFactoryType;
        }

        internal static bool InspectGenerateConstructor(AttributeData viewModelAttributeData)
        {
            var generateConstructor = true;

            foreach (var arg in viewModelAttributeData.NamedArguments)
            {
                if (arg.Key == "GenerateConstructor")
                {
                    generateConstructor = (bool?)arg.Value.Value == true;
                }
            }

            return generateConstructor;
        }

        internal static string? InspectModelPropertyName(AttributeData viewModelAttributeData)
        {
            string? modelPropertyName = null;

            foreach (var arg in viewModelAttributeData.NamedArguments)
            {
                if (arg.Key == "ModelPropertyName")
                {
                    modelPropertyName = arg.Value.Value?.ToString();
                }
            }

            return modelPropertyName;
        }
    }
}
