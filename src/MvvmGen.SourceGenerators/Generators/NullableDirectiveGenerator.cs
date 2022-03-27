// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

namespace MvvmGen.Generators
{
    internal static class NullableDirectiveGenerator
    {
        internal static void GenerateNullableDirective(this ViewModelBuilder vmBuilder)
        {
            vmBuilder.AppendLine("#nullable enable");
        }
    }
}
