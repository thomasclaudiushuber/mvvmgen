// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class PropertyGenerator
    {
        internal static void Generate(IEnumerable<PropertyToGenerate>? propertiesToGenerate, StringBuilder stringBuilder, string indent)
        {
            if (propertiesToGenerate is not null)
            {
                foreach (var propertyToGenerate in propertiesToGenerate)
                {
                    GenerateProperty(propertyToGenerate, stringBuilder, indent);
                }
            }
        }

        private static void GenerateProperty(PropertyToGenerate p, StringBuilder stringBuilder, string indent)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append(indent + $"public {p.PropertyType} {p.PropertyName}");

            if (p.IsReadOnly)
            {
                stringBuilder.AppendLine($" => {p.BackingField};");
                return;
            }
            else
            {
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine(indent + $"{{");
            stringBuilder.AppendLine(indent + $"    get => {p.BackingField};");
            stringBuilder.AppendLine(indent + $"    set");
            stringBuilder.AppendLine(indent + $"    {{");
            stringBuilder.AppendLine(indent + $"        if ({p.BackingField} != value)");
            stringBuilder.AppendLine(indent + $"        {{");
            stringBuilder.AppendLine(indent + $"            {p.BackingField} = value;");
            stringBuilder.AppendLine(indent + $"            OnPropertyChanged(\"{p.PropertyName}\");");
            if (p.CommandsToInvalidate is not null)
            {
                foreach (var commandToInvalidate in p.CommandsToInvalidate)
                {
                    stringBuilder.AppendLine(indent + $"            {commandToInvalidate.CommandName}.RaiseCanExecuteChanged();");
                }
            }
            if (p.EventToPublish is { Length: > 0 })
            {
                stringBuilder.AppendLine(indent + $"            EventAggregator.Publish(new {p.EventToPublish}({p.EventToPublishConstructorArgs}));");
            }
            stringBuilder.AppendLine(indent + $"        }}");
            stringBuilder.AppendLine(indent + $"    }}");
            stringBuilder.AppendLine(indent + $"}}");
        }
    }
}
