// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Collections.Generic;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class CommandPropertyGenerator
    {
        internal static void GenerateCommandProperties(this ViewModelBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate is not null)
            {
                foreach (var comandToGenerate in commandsToGenerate)
                {
                    vmBuilder.AppendLineBeforeMember();
                    vmBuilder.AppendLine($"public DelegateCommand {comandToGenerate.CommandName} {{ get; private set; }}");
                }
            }
        }
    }
}
