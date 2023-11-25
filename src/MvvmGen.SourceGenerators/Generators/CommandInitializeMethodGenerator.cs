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
    internal static class CommandInitializeMethodGenerator
    {
        internal static void GenerateCommandInitializeMethod(
            this ViewModelBuilder vmBuilder, 
            IEnumerable<CommandToGenerate>? commandsToGenerate,
            string? commandFactoryType)
        {
            if (commandsToGenerate is not null && commandsToGenerate.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine("private void InitializeCommands()");
                vmBuilder.AppendLine("{");
                vmBuilder.IncreaseIndent();
                
                commandFactoryType ??= "DelegateCommandFactory";
                vmBuilder.AppendLine($"var commandFactory = new {commandFactoryType}();");

                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.Append($"{commandToGenerate.PropertyName} = commandFactory.CreateCommand({GetMethodCall(commandToGenerate.ExecuteMethod)}");
                    if (commandToGenerate.CanExecuteMethod is not null)
                    {
                        vmBuilder.Append($", {GetMethodCall(commandToGenerate.CanExecuteMethod)}");
                    }
                    else
                    {
                        vmBuilder.Append(", null");
                    }

                    vmBuilder.AppendLine(");");
                }

                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
            }
        }

        private static object GetMethodCall(CommandMethod methodInfo)
        {
            return methodInfo switch
            {
                { IsAwaitable: true, HasParameter: true } => $"async x => await {methodInfo.Name}(x)",
                { IsAwaitable: true, HasParameter: false } => $"async _ => await {methodInfo.Name}()",
                { IsAwaitable: false, HasParameter: true } => $"{methodInfo.Name}",
                { IsAwaitable: false, HasParameter: false } => $"_ => {methodInfo.Name}()",
            };
        }
    }
}
