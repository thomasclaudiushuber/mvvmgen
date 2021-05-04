// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System.ComponentModel;
using System.Runtime.CompilerServices;
using MvvmGen.Events;

namespace MvvmGen.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            OnInitialize();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnInitialize() { }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
