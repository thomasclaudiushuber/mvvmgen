﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class PropertyGenerator
    {
        internal static void Generate(ViewModelBuilder vmBuilder, IEnumerable<PropertyToGenerate>? propertiesToGenerate)
        {
            if (propertiesToGenerate is not null)
            {
                foreach (var propertyToGenerate in propertiesToGenerate)
                {
                    GenerateProperty(vmBuilder, propertyToGenerate);
                }
            }
        }

        private static void GenerateProperty(ViewModelBuilder vmBuilder, PropertyToGenerate p)
        {
            vmBuilder.AppendLineBeforeMember();
            vmBuilder.Append($"public {p.PropertyType} {p.PropertyName}");

            if (p.IsReadOnly)
            {
                vmBuilder.AppendLine($" => {p.BackingField};");
                return;
            }
            else
            {
                vmBuilder.AppendLine();
            }

            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"get => {p.BackingField};");
            vmBuilder.AppendLine("set");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"if ({p.BackingField} != value)");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            vmBuilder.AppendLine($"{p.BackingField} = value;");
            vmBuilder.AppendLine($"OnPropertyChanged(\"{p.PropertyName}\");");
            if (p.CommandsToInvalidate is not null)
            {
                foreach (var commandToInvalidate in p.CommandsToInvalidate)
                {
                    vmBuilder.AppendLine($"{commandToInvalidate.CommandName}.RaiseCanExecuteChanged();");
                }
            }
            if (p.EventsToPublish is not null)
            {
                foreach (var eventToPublish in p.EventsToPublish)
                {
                    vmBuilder.AppendLine($"{eventToPublish.EventAggregatorMemberName}.Publish(new {eventToPublish.EventType}({eventToPublish.EventConstructorArgs}));");
                }
            }
            if (p.MethodsToCall is not null)
            {
                foreach (var methodToCall in p.MethodsToCall)
                {
                    vmBuilder.AppendLine($"{methodToCall.MethodName}({methodToCall.MethodArgs});");
                }
            }
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
        }
    }
}