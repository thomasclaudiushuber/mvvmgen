// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;

namespace MvvmGen.Commands
{
    public interface IDelegateCommandFactory
    {
        IDelegateCommand CreateCommand(Action<object?> execute, Func<object?, bool>? canExecute);
    }
}
