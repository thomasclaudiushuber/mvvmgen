// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

using System.Collections.Generic;
using System.Linq;
using MvvmGen.SourceGenerators.Model;

namespace MvvmGen.SourceGenerators
{
    internal static class CommandInitializeMethodGenerator
    {
        internal static void GenerateCommandInitializeMethod(this ViewModelBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate is not null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine("private void InitializeCommands()");
                vmBuilder.AppendLine("{");
                vmBuilder.IncreaseIndent();
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.Append($"{commandToGenerate.CommandName} = new DelegateCommand({GetMethodCall(commandToGenerate.ExecuteMethod)}");
                    if (commandToGenerate.CanExecuteMethod.HasValue)
                    {
                        vmBuilder.Append($", {GetMethodCall(commandToGenerate.CanExecuteMethod.Value)}");
                    }
                    vmBuilder.AppendLine(");");
                }
                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
            }
        }

        private static object GetMethodCall(MethodInfo methodInfo)
        {
            return methodInfo switch
            {
                { IsAsync: true, HasParameter: true } => $"async x => await {methodInfo.Name}(x)",
                { IsAsync: true, HasParameter: false } => $"async _ => await {methodInfo.Name}()",
                { IsAsync: false, HasParameter: true } => $"{methodInfo.Name}",
                { IsAsync: false, HasParameter: false } => $"_ => {methodInfo.Name}()",
            };
        }
    }
}
