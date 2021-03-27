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
      Initialize();
      Model = new Employee
      {
        FirstName = "Thomas Claudius",
        LastName = "Huber",
        IsDeveloper = true
      };
    }

    [Command(nameof(CanSave))]
    public void Save()
    {

    }

    [Invalidate(nameof(UpdateComment))]
    [Invalidate(nameof(FirstName))]
    public bool CanSave()
    {
      return !string.IsNullOrEmpty(FirstName)
        && !string.IsNullOrEmpty(UpdateComment);
    }
  }
}
