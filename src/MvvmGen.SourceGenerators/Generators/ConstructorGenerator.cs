// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using MvvmGen.Extensions;
using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class ConstructorGenerator
    {
        internal static void GenerateConstructor(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate)
        {
            if (viewModelToGenerate.GenerateConstructor)
            {
                Generate(vmBuilder, viewModelToGenerate.ClassName,
                            viewModelToGenerate.InjectionsToGenerate,
                            viewModelToGenerate.BaseClassInjectionsToGenerate,
                            viewModelToGenerate.IsEventSubscriber);
            }
        }

        private static void Generate(ViewModelBuilder vmBuilder, string viewModelClassName,
            IEnumerable<InjectionToGenerate>? directInjectionsToGenerate,
            IEnumerable<InjectionToGenerate>? baseClassInjectionsToGenerate,
            bool isEventSubscriber)
        {
            vmBuilder.AppendLineBeforeMember();
            vmBuilder.Append($"public {viewModelClassName}(");
            directInjectionsToGenerate ??= Enumerable.Empty<InjectionToGenerate>();
            baseClassInjectionsToGenerate ??= Enumerable.Empty<InjectionToGenerate>();

            var first = true;
            string? eventAggregatorAccessForSubscription = null;
            if (isEventSubscriber)
            {
                var eventAggregatorInjection = directInjectionsToGenerate.FirstOrDefault(x => x.Type == "MvvmGen.Events.IEventAggregator");
                if (eventAggregatorInjection is not null)
                {
                    eventAggregatorAccessForSubscription = $"this.{eventAggregatorInjection.PropertyName}";
                }
                else
                {
                    eventAggregatorAccessForSubscription = "eventAggregator";
                    first = false;
                    vmBuilder.Append($"MvvmGen.Events.IEventAggregator {eventAggregatorAccessForSubscription}");
                }
            }

            foreach (var injectionToGenerate in directInjectionsToGenerate)
            {
                if (!first)
                {
                    vmBuilder.Append(", ");
                }
                first = false;
                vmBuilder.Append($"{injectionToGenerate.Type} {injectionToGenerate.PropertyName.ToCamelCase()}");
            }

            var hasBaseClassInjections = false;
            foreach (var injectionToGenerate in baseClassInjectionsToGenerate)
            {
                hasBaseClassInjections = true;
                if (!first)
                {
                    vmBuilder.Append(", ");
                }
                first = false;
                vmBuilder.Append($"{injectionToGenerate.Type} {injectionToGenerate.PropertyName.ToCamelCase()}");
            }

            vmBuilder.Append(")");

            if (hasBaseClassInjections)
            {
                vmBuilder.Append(" : base(");

                first = true;
                foreach (var injectionToGenerate in baseClassInjectionsToGenerate)
                {
                    if (!first)
                    {
                        vmBuilder.Append(", ");
                    }
                    first = false;
                    vmBuilder.Append(injectionToGenerate.PropertyName.ToCamelCase());
                }

                vmBuilder.Append(")");
            }

            vmBuilder.AppendLine();
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            foreach (var injectionToGenerate in directInjectionsToGenerate)
            {
                vmBuilder.AppendLine($"this.{injectionToGenerate.PropertyName} = {injectionToGenerate.PropertyName.ToCamelCase()};");
            }

            if (isEventSubscriber)
            {
                vmBuilder.AppendLine($"{eventAggregatorAccessForSubscription}.RegisterSubscriber(this);");
            }

            vmBuilder.AppendLine($"this.OnInitialize();");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"partial void OnInitialize();");
        }
    }
}
