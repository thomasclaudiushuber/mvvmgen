// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

using System;
using System.Windows.Input;

namespace MvvmGen.Commands
{
    /// <summary>
    /// An <see cref="ICommand"/> implementation that works with delegates for the execute and can-execute logic
    /// </summary>
    public class DelegateCommand : IDelegateCommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">The function that determines whether the command can execute</param>
        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <inheritdoc/>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        /// <inheritdoc/>
        public void Execute(object? parameter) => _execute(parameter);

        /// <inheritdoc/>
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

    }
}
