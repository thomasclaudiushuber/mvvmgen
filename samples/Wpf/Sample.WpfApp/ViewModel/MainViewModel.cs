using System;
using System.Collections.ObjectModel;
using System.Linq;
using MvvmGen;
using MvvmGen.Events;
using MvvmGen.ViewModels;
using Sample.WpfApp.Events;

namespace Sample.WpfApp.ViewModel
{
    [Inject(typeof(IEmployeeViewModelFactory))]
    [Inject(typeof(NavigationViewModel), PropertyAccessModifier = AccessModifier.Public)]
    [Inject(typeof(IEventAggregator))]
    [ViewModel]
    public partial class MainViewModel : IEventSubscriber<EmployeeSelectedEvent>
    {
        public void OnEvent(EmployeeSelectedEvent eventData)
        {
            var employeeViewModel = EmployeeViewModels.SingleOrDefault(x => x.Id == eventData.EmployeeId);
            if (employeeViewModel is null)
            {
                employeeViewModel = EmployeeViewModelFactory.Create();
                employeeViewModel.Load(eventData.EmployeeId);
                EmployeeViewModels.Add(employeeViewModel);
            }

            SelectedEmployee = employeeViewModel;
        }

        internal void Load()
        {
            NavigationViewModel.Load();
        }

        public ObservableCollection<EmployeeViewModel> EmployeeViewModels { get; } = new();

        [Property]
        private EmployeeViewModel selectedEmployee;
    }
}
