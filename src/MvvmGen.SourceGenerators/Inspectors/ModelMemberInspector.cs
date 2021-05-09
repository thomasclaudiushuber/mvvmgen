// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Inspectors
{
    internal static class ModelMemberInspector
    {
        internal static string? Inspect(AttributeData viewModelAttributeData, IList<PropertyToGenerate> propertiesToGenerate)
        {
            string? wrappedModelType = null;

            var modelTypedConstant = (TypedConstant?)viewModelAttributeData.ConstructorArguments.FirstOrDefault();
            foreach (var arg in viewModelAttributeData.NamedArguments)
            {
                if (arg.Key == "ModelType")
                {
                    modelTypedConstant = arg.Value;
                }
            }
            if (modelTypedConstant?.Value is not null)
            {
                if (modelTypedConstant.Value.Value is INamedTypeSymbol model)
                {
                    wrappedModelType = $"{model}";
                    var members = model.GetMembers();
                    foreach (var member in members)
                    {
                        if (member is IMethodSymbol { MethodKind: MethodKind.PropertyGet } methodSymbol)
                        {
                            var propertySymbol = (IPropertySymbol?)methodSymbol.AssociatedSymbol;
                            if (propertySymbol is not null)
                            {
                                propertiesToGenerate.Add(new PropertyToGenerate(
                                  propertySymbol.Name, propertySymbol.Type.ToString(), $"Model.{propertySymbol.Name}", propertySymbol.IsReadOnly));
                            }
                        }
                    }
                }
            }

            return wrappedModelType;
        }
    }
}
