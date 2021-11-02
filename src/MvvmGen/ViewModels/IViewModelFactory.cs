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
    /// <typeparam name="T">The type of ViewModel to create. Must be of type <see cref="ViewModelBase"/></typeparam>
    public interface IViewModelFactory<out T> where T : global::MvvmGen.ViewModels.ViewModelBase
    {
        /// <summary>
        /// Creates and returns a ViewModel instance.
        /// </summary>
        /// <returns>The created ViewModel instance</returns>
        T Create();
    }
}
