// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class ViewModelInterfaceGenerator
    {
        internal static void GenerateViewModelInterface(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate)
        {
            if (viewModelToGenerate.ViewModelInterfaceToGenerate is null)
            {
                return;
            }

            var interfaceToGenerate = viewModelToGenerate.ViewModelInterfaceToGenerate;

            var accessModifier = viewModelToGenerate.ClassAccessModifier;

            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"{accessModifier} interface {interfaceToGenerate.InterfaceName}");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();

            if (interfaceToGenerate.Properties is not null)
            {
                foreach (var property in interfaceToGenerate.Properties)
                {
                    vmBuilder.AppendLine($"{property.PropertyType} {property.PropertyName} {{ get; {(property.IsReadOnly ? "" : "set; ")}}}");
                }
            }

            if (interfaceToGenerate.Methods is not null)
            {
                foreach (var method in interfaceToGenerate.Methods)
                {
                    vmBuilder.Append($"{method.ReturnType} {method.MethodName}");

                    if (method.IsGeneric)
                    {
                        vmBuilder.Append("<");
                        if (method.GenericTypeParameters is not null)
                        {
                            var first = true;
                            foreach (var typeParam in method.GenericTypeParameters)
                            {
                                if (!first)
                                {
                                    vmBuilder.Append(", ");
                                }

                                vmBuilder.Append(typeParam);
                                first = false;
                            }
                        }
                        vmBuilder.Append(">");
                    }

                    vmBuilder.Append("(");

                    if (method.Parameters is not null)
                    {
                        var isFirst = true;

                        foreach (var parameter in method.Parameters)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                            }
                            else
                            {
                                vmBuilder.Append(", ");
                            }

                            vmBuilder.Append($"{parameter.ParameterName} {parameter.ParameterType}");
                        }
                    }

                    vmBuilder.Append(")");

                    if (method.IsGeneric && method.GenericTypeConstraints is { Length: > 0 })
                    {
                        foreach (var genericTypeConstraint in method.GenericTypeConstraints)
                        {
                            vmBuilder.Append($" {genericTypeConstraint}");
                        }
                    }

                    vmBuilder.AppendLine(";");
                }
            }

            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
        }
    }
}
