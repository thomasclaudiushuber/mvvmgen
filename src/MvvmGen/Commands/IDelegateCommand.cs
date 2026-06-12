// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System.Windows.Input;

namespace MvvmGen.Commands
{
    /// <summary>
    /// Defines a delegate command.
    /// </summary>
    public interface IDelegateCommand : ICommand
    {
        /// <summary>
        /// Raises the <see cref="ICommand.CanExecuteChanged"/> event.
        /// </summary>
        void RaiseCanExecuteChanged();
    }
}
