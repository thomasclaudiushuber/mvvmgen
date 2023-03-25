// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class NamespaceGenerator
    {
        internal static void GenerateNamespace(this ViewModelBuilder vmBuilder, ViewModelToGenerate viewModelToGenerate)
        {
            vmBuilder.AppendLine();
            vmBuilder.AppendLine($"namespace {viewModelToGenerate.NamespaceName}");
            vmBuilder.AppendLine("{");
            vmBuilder.IncreaseIndent();
        }
    }
}
