using Sample.Model;
using MvvmGen;
using System;
using MvvmGen.Core;

namespace Sample.WpfApp.ViewModel
{
  [ViewModelGenerator(typeof(Employee))]
  public partial class EmployeeViewModel
  {
    public void Save() { }
  }
}
