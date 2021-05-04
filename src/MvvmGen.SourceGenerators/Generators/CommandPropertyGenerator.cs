// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using System.Text;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class CommandPropertyGenerator
    {
        internal static void Generate(IEnumerable<CommandToGenerate>? commandsToGenerate, StringBuilder stringBuilder, string indent)
        {
            if (commandsToGenerate is not null)
            {
                foreach (var comandToGenerate in commandsToGenerate)
                {
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine($"{indent}public DelegateCommand {comandToGenerate.CommandName} {{ get; private set; }}");
                }
            }
        }
    }
}
