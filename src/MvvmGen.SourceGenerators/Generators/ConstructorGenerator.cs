// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmGen.SourceGenerators.Extensions;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class ConstructorGenerator
    {
        internal static void Generate(ViewModelBuilder vmBuilder, string viewModelClassName,
            IEnumerable<InjectionToGenerate>? injectionsToGenerate,
            bool hasCommands)
        {
            vmBuilder.AppendLineBeforeMember();
            vmBuilder.Append($"public {viewModelClassName}(");
            injectionsToGenerate ??= Enumerable.Empty<InjectionToGenerate>();

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

            if (hasCommands)
            {
                vmBuilder.AppendLine($"this.InitializeCommands();");
            }

            vmBuilder.AppendLine($"this.OnInitialize();");
            vmBuilder.DecreaseIndent();
            vmBuilder.AppendLine("}");
            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"partial void OnInitialize();");


        }
    }
}
