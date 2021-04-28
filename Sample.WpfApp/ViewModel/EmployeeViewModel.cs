using Sample.Model;
using MvvmGen;
using System;
using MvvmGen.Core;

namespace Sample.WpfApp.ViewModel
{
  [ViewModel(ModelType = typeof(Employee))]
  public partial class EmployeeViewModel
  {
    public EmployeeViewModel()
    {
      InitializeCommands();
    }

    [Command(CanExecuteMethod = nameof(CanSave))]
    public void Save()
    {

    }

    [InvalidateOnPropertyChange(nameof(FirstName))]
    [InvalidateOnPropertyChange(nameof(LastName))]
    public bool CanSave()
    {
      return string.IsNullOrEmpty(FirstName)
        && string.IsNullOrEmpty(LastName);
    }
  }
}
