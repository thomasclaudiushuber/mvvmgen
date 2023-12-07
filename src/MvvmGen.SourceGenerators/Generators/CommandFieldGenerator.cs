// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class CommandFieldGenerator
    {
        internal static void GenerateCommandFields(this ViewModelBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate is not null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.AppendLine($"private IDelegateCommand? {commandToGenerate.FieldName};");
                }
            }
        }
    }
}
