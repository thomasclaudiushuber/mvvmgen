// ********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in project root
// ********************************************************************

namespace MvvmGen.SourceGenerators.Model
{
    internal class CommandToGenerate
    {
        public CommandToGenerate(MethodInfo executeMethod, string commandName)
        {
            ExecuteMethod = executeMethod;
            CommandName = commandName;
        }

        public MethodInfo ExecuteMethod { get; }

        public string CommandName { get; }

        public MethodInfo? CanExecuteMethod { get; set; }

        public string[]? CanExecuteAffectingProperties { get; set; }
    }

    internal struct MethodInfo
    {
        public MethodInfo(string name) : this()
        {
            Name = name;
        }

        public string Name { get; set; }

        public bool IsAsync { get; set; }

        public bool HasParameter { get; set; }
    }
}
