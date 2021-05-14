// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Collections.Generic;
using MvvmGen.Model;

namespace MvvmGen
{
    internal static class InjectionPropertyGenerator
    {
        internal static void GenerateInjectionProperties(this ViewModelBuilder vmBuilder, IEnumerable<InjectionToGenerate>? injectionsToGenerate)
        {
            if (injectionsToGenerate is not null)
            {
                foreach (var injectionToGenerate in injectionsToGenerate)
                {
                    vmBuilder.AppendLineBeforeMember();
                    vmBuilder.AppendLine($"{injectionToGenerate.PropertyAccessModifier} {injectionToGenerate.Type} {injectionToGenerate.PropertyName} {{ get; private set; }}");
                }
            }
        }
    }
}
