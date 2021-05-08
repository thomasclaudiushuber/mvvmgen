// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.ViewModels
{
    /// <summary>
    /// Represents a factory that can create view model instances with a parameterless <see cref="Create"/> method
    /// </summary>
    /// <typeparam name="T">The type of view model to create</typeparam>
    public interface IViewModelFactory<out T> where T : ViewModelBase
    {
        /// <summary>
        /// Creates and returns a view model instance
        /// </summary>
        /// <returns>The created view model</returns>
        T Create();
    }
}
