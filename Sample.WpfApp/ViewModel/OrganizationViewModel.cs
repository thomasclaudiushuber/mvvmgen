using MvvmGen;
using Sample.Model;

namespace Sample.WpfApp.ViewModel
{
  [ViewModelGeneratorAttribute(typeof(Organization))]
  public partial class OrganizationViewModel
  {
    public OrganizationViewModel()
    {
    }
  }
}
