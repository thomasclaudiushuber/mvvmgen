using MvvmGen;
using MvvmGen.Events;
using Sample.WpfApp.Events;
using Sample.WpfApp.Model;

namespace Sample.WpfApp.ViewModel
{
    [ViewModel(typeof(Employee))]
    public partial class EmployeeViewModel
    {
        [Property]
        private string? _updateComment;
        private readonly IEventAggregator _eventAggregator;

        public EmployeeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public EmployeeViewModel()
        {
            Model = new Employee
            {
                FirstName = "Thomas Claudius",
                LastName = "Huber",
                IsDeveloper = true
            };
        }

        [Command(CanExecuteMethod = nameof(CanSave))]
        public void Save()
        {
            _eventAggregator.Publish(new EmployeeSavedEvent(Model.Id, Model.FirstName));
        }

        [CommandInvalidate(nameof(UpdateComment))]
        [CommandInvalidate(nameof(FirstName))]
        public bool CanSave()
        {
            return !string.IsNullOrEmpty(FirstName)
              && !string.IsNullOrEmpty(UpdateComment);
        }
    }
}
