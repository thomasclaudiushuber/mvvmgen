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
using MvvmGen.SourceGenerators.Extensions;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class ViewModelFactoryGenerator
    {
        internal static void GenerateFactoryClass(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate)
        {
            Generate(vmBuilder, viewModelToGenerate.ViewModelClassSymbol,
                viewModelToGenerate.InjectionsToGenerate);
        }

        private static void Generate(ViewModelBuilder vmBuilder, INamedTypeSymbol viewModelClassSymbol,
               IEnumerable<InjectionToGenerate>? injectionsToGenerate)
        {
            injectionsToGenerate ??= Enumerable.Empty<InjectionToGenerate>();
            string viewModelClassName = viewModelClassSymbol.Name;
            string accessModifier = viewModelClassSymbol.DeclaredAccessibility switch
            {
                Accessibility.Public => "public",
                Accessibility.Internal => "internal",
                _ => ""
            };
            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"{accessModifier} interface I{viewModelClassName}Factory : IViewModelFactory<{viewModelClassName}> {{ }}");
            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"{accessModifier} class {viewModelClassName}Factory : I{viewModelClassName}Factory");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();

            vmBuilder.Append($"public {viewModelClassName}Factory(");
            bool first = true;
            foreach (var injectionToGenerate in injectionsToGenerate)
            {
                if (!first)
                {
                    vmBuilder.Append(", ");
                }
                first = false;
                vmBuilder.Append($"{injectionToGenerate.Type} {injectionToGenerate.PropertyName.PascalCaseToCamelCase()}");
            }

            vmBuilder.AppendLine(")");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
            foreach (var injectionToGenerate in injectionsToGenerate)
            {
                vmBuilder.AppendLine($"this.{injectionToGenerate.PropertyName} = {injectionToGenerate.PropertyName.PascalCaseToCamelCase()};");
            }

            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");

            InjectionPropertyGenerator.GenerateInjectionProperties(vmBuilder, injectionsToGenerate);

            vmBuilder.AppendLine();
            vmBuilder.Append($"public {viewModelClassName} Create() => new {viewModelClassName}(");
            first = true;
            foreach (var injectionToGenerate in injectionsToGenerate)
            {
                if (!first)
                {
                    vmBuilder.Append(", ");
                }
                first = false;
                vmBuilder.Append(injectionToGenerate.PropertyName);
            }
            vmBuilder.AppendLine(");");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
        }
    }
}
