// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using MvvmGen.Model;

namespace MvvmGen.Inspectors
{
    internal static class ModelMemberInspector
    {
        internal static string? Inspect(AttributeData viewModelAttributeData,
            IList<PropertyToGenerate> propertiesToGenerate,
            string? wrappedModelPropertyName,
            string? wrappedModelPropertiesToIgnore)
        {
            string? wrappedModelType = null;
            var wrappedModelPropsToIgnore = wrappedModelPropertiesToIgnore?
                                               .Split(',').Select(x => x.Trim()).ToList();

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
                    wrappedModelPropertyName = wrappedModelPropertyName ?? "Model";
                    var members = GetAllMembers(model);
                    foreach (var member in members)
                    {
                        if (member is IMethodSymbol { MethodKind: MethodKind.PropertyGet } methodSymbol)
                        {
                            var propertySymbol = (IPropertySymbol?)methodSymbol.AssociatedSymbol;
                            if (propertySymbol is not null)
                            {
                                if (wrappedModelPropsToIgnore is not null
                                 && wrappedModelPropsToIgnore.Contains(propertySymbol.Name))
                                {
                                    continue;
                                }

                                propertiesToGenerate.Add(new PropertyToGenerate(
                                  propertySymbol.Name, propertySymbol.Type.ToString(), $"{wrappedModelPropertyName}.{propertySymbol.Name}", propertySymbol.IsReadOnly));
                            }
                        }
                    }
                }
            }

            return wrappedModelType;
        }

        private static IEnumerable<ISymbol> GetAllMembers(INamedTypeSymbol model)
        {
            List<ISymbol> members = new(model.GetMembers());
            while (model.BaseType is not null)
            {
                members.InsertRange(0, model.BaseType.GetMembers());
                model = model.BaseType;
            }

            return members;
        }
    }
}
