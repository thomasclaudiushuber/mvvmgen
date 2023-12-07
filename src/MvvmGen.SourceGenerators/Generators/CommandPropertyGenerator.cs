// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using MvvmGen.Model;

namespace MvvmGen.Generators
{
    internal static class CommandPropertyGenerator
    {
        internal static void GenerateCommandProperties(this ViewModelBuilder vmBuilder, IEnumerable<CommandToGenerate>? commandsToGenerate)
        {
            if (commandsToGenerate is not null)
            {
                foreach (var commandToGenerate in commandsToGenerate)
                {
                    vmBuilder.AppendLineBeforeMember();
                    vmBuilder.Append($"public IDelegateCommand {commandToGenerate.PropertyName} => {commandToGenerate.FieldName} ??= new DelegateCommand({GetMethodCall(commandToGenerate.ExecuteMethod)}");
                    if (commandToGenerate.CanExecuteMethod is not null)
                    {
                        vmBuilder.Append($", {GetMethodCall(commandToGenerate.CanExecuteMethod)}");
                    }

                    vmBuilder.AppendLine(");");
                }
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
