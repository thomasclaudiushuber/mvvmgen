using Sample.Model;
using MvvmGen;

namespace Sample.WpfApp.ViewModel
{
  [ViewModel(typeof(Employee))]
  public partial class EmployeeViewModel
  {
    [Property]
    private string? _updateComment;

    public EmployeeViewModel()
    {
      InitializeCommands();
    }

    [Command(nameof(CanSave))]
    public void Save()
    {

    }

    [Invalidate(nameof(UpdateComment))]
    [Invalidate(nameof(FirstName))]
    [Invalidate(nameof(LastName))]
    public bool CanSave()
    {
      return !string.IsNullOrEmpty(FirstName);
    }
  }
}
