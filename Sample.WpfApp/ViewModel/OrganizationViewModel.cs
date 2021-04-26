using MvvmGen;
using Sample.Model;

namespace Sample.WpfApp.ViewModel
{
  [ViewModelGenerator(typeof(Organization))]
  public partial class OrganizationViewModel
  {
    public OrganizationViewModel()
    {
    }
  }
}
