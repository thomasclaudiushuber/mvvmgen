// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

namespace MvvmGen.Generators
{
    internal static class CommandInvalidateMethodGenerator
    {
        internal static void GenerateInvalidateCommandsMethod(this ViewModelBuilder vmBuilder,
            IDictionary<string, List<string>>? commandsByPropertyNames)
        {
            if (commandsByPropertyNames is not null && commandsByPropertyNames.Any())
            {
                vmBuilder.AppendLineBeforeMember();
                vmBuilder.AppendLine("protected override void InvalidateCommands(string? propertyName)");
                vmBuilder.AppendLine("{");
                vmBuilder.IncreaseIndent();
                vmBuilder.AppendLine("base.InvalidateCommands(propertyName);");

                var first = true;
                var ifStatement = "if";

                foreach (var commandsByPropertyName in commandsByPropertyNames)
                {
                    vmBuilder.AppendLine(@$"{ifStatement} (propertyName == ""{commandsByPropertyName.Key}"")");
                    if (first)
                    {
                        first = false;
                        ifStatement = "else if";
                    }
                    vmBuilder.AppendLine("{");
                    vmBuilder.IncreaseIndent();
                    foreach (var commandName in commandsByPropertyName.Value)
                    {
                        vmBuilder.AppendLine($"{commandName}.RaiseCanExecuteChanged();");
                    }

                    vmBuilder.DecreaseIndent();
                    vmBuilder.AppendLine("}");
                }

                vmBuilder.DecreaseIndent();
                vmBuilder.AppendLine("}");
            }
        }
    }
}
