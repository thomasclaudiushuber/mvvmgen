using MvvmGen;
using MvvmGen.Events;
using Sample.WpfApp.DataProvider;
using Sample.WpfApp.Events;
using Sample.WpfApp.Model;

namespace Sample.WpfApp.ViewModel
{
    [Inject(typeof(IEmployeeDataProvider))]
    [Inject(typeof(IEventAggregator))]
    [ViewModelGenerateFactory]
    [ViewModel(typeof(Employee))]
    public partial class EmployeeViewModel
    {
        [Property]
        private string? _updateComment;

        [Command(CanExecuteMethod = nameof(CanSave))]
        public void Save()
        {
            EventAggregator.Publish(new EmployeeSavedEvent(Model.Id, Model.FirstName!));
        }

        [CommandInvalidate(nameof(UpdateComment))]
        [CommandInvalidate(nameof(FirstName))]
        public bool CanSave()
        {
            return !string.IsNullOrEmpty(FirstName)
              && !string.IsNullOrEmpty(UpdateComment);
        }

        internal void Load(int employeeId)
        {
            Model = EmployeeDataProvider.GetById(employeeId);
        }
    }
}
