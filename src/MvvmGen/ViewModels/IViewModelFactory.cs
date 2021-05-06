// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.ViewModels
{
    public interface IViewModelFactory<out T> where T : ViewModelBase
    {
        T Create();
    }
}
