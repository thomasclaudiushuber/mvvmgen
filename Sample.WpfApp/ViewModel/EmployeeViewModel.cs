using Sample.Model;
using MvvmGen;

namespace Sample.WpfApp.ViewModel
{
  [ViewModelGenerator(typeof(Employee))]
  public partial class EmployeeViewModel
  {
    public EmployeeViewModel()
    {
      
    }
  }
}
