// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

namespace MvvmGen.Generators
{
    internal static class ModelPropertyGenerator
    {
        internal static void GenerateModelProperty(this ViewModelBuilder vmBuilder, string? wrappedModelType, string? wrappedModelPropertyName)
        {
            if (wrappedModelType is { Length: > 0 })
            {
                var modelPropertyName = wrappedModelPropertyName ?? "Model";
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine($"protected {wrappedModelType} {modelPropertyName} {{ get; set; }}");
            }
        }
    }
}
