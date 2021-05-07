// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class CommandInitializeMethodGenerator
    {
        internal static void Generate(ViewModelBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate is not null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine("private void InitializeCommands()");
                vmBuilder.AppendLine("{");
                vmBuilder.IncreaseIndent();
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.Append($"{commandToGenerate.CommandName} = new(_ => {commandToGenerate.ExecuteMethod}()");
                    if (commandToGenerate.CanExecuteMethod is not null)
                    {
                        vmBuilder.Append($", _ => {commandToGenerate.CanExecuteMethod}()");
                    }
                    vmBuilder.AppendLine(");");
                }
                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
            }
        }
    }
}
