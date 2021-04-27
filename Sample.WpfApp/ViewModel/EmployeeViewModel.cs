using Sample.Model;
using MvvmGen;
using System;

namespace Sample.WpfApp.ViewModel
{
  [ViewModelGenerator(typeof(Employee))]
  public partial class EmployeeViewModel
  {
    public EmployeeViewModel()
    {
      
    }

    public void Save()
    {
      
    }

    public bool CanSave()
    {
      return true;
    }
  }
}
