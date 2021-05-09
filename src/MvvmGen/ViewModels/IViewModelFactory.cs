// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.ViewModels
{
    /// <summary>
    /// Represents a factory that can create ViewModel instances with a parameterless <see cref="Create"/> method.
    /// </summary>
    /// <typeparam name="T">The type of ViewModel to create. Must be of type <see cref="ViewModelBase"/></typeparam>
    public interface IViewModelFactory<out T> where T : ViewModelBase
    {
        /// <summary>
        /// Creates and returns a ViewModel instance.
        /// </summary>
        /// <returns>The created ViewModel instance</returns>
        T Create();
    }
}
