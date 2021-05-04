// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Text;

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class ModelPropertyGenerator
    {
        internal static void Generate(string? wrappedModelType, StringBuilder stringBuilder, string indent)
        {
            if (wrappedModelType is { Length: >0 })
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"{indent}public {wrappedModelType} Model {{ get; set; }}");
            }
        }
    }
}
