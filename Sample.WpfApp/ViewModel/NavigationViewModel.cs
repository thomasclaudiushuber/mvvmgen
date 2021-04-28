using MvvmGen;
using Sample.Model;
using Sample.WpfApp.DataProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.WpfApp.ViewModel
{
  [ViewModel]
  public partial class NavigationViewModel
  {
    public NavigationViewModel(IEmployeeDataProvider employeeDataProvider)
    {
      //InitializeCommands();
    }

    public void Refresh()
    {

    }



    public ObservableCollection<NavigationEmployeeItem> Employees { get; } = new();
    public ObservableCollection<NavigationOrganizationItem> Organizations { get; } = new();
  }

  [ViewModel(typeof(Employee))]
  public partial class NavigationEmployeeItem { }

  [ViewModel(typeof(Organization))]
  public partial class NavigationOrganizationItem { }
}
