// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

namespace MvvmGen.SourceGenerators.Generators
{
    internal static class UsingDirectivesGenerator
    {
        internal static void GenerateUsingDirectives(this ViewModelBuilder vmBuilder)
        {
            vmBuilder.AppendLine("using MvvmGen.Commands;");
            vmBuilder.AppendLine("using MvvmGen.Events;");
            vmBuilder.AppendLine("using MvvmGen.ViewModels;");
        }
    }
}
