// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.SourceGenerators.Model
{
    internal class CommandToGenerate
    {
        public CommandToGenerate(string executeMethod, string commandName)
        {
            ExecuteMethod = executeMethod;
            CommandName = commandName;
        }

        public string ExecuteMethod { get; }
        public string CommandName { get; }
        public string? CanExecuteMethod { get; set; }
        public string[]? CanExecuteAffectingProperties { get; set; }
    }
}
