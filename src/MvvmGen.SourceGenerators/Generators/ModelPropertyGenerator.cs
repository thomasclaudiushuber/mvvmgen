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
        internal static void Generate(ViewModelBuilder vmBuilder, string? wrappedModelType)
        {
            if (wrappedModelType is { Length: >0 })
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine($"protected {wrappedModelType} Model {{ get; set; }}");
            }
        }
    }
}
