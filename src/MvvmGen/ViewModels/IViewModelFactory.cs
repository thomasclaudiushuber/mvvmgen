// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

#nullable enable

namespace MvvmGen.ViewModels
{
    /// <summary>
    /// Represents a factory that can create ViewModel instances with a parameterless <see cref="Create"/> method.
    /// You get a generated implementation of this interface by setting the <see cref="ViewModelGenerateFactoryAttribute"/> on your ViewModel.
    /// </summary>
    public interface IViewModelFactory<out T>
    {
        /// <summary>
        /// Creates and returns a ViewModel instance.
        /// </summary>
        /// <returns>The created ViewModel instance</returns>
        T Create();
    }
}
