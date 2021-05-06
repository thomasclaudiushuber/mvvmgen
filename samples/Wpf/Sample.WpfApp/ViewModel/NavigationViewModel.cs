using MvvmGen;
using MvvmGen.Events;
using MvvmGen.ViewModels;
using Sample.WpfApp.DataProvider;
using Sample.WpfApp.Events;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sample.WpfApp.ViewModel
{
    [Inject(typeof(IEmployeeDataProvider))]
    [Inject(typeof(IEventAggregator))]
    [ViewModel]
    public partial class NavigationViewModel : IEventSubscriber<EmployeeSavedEvent>
    {
        [OnChangePublishEvent(typeof(EmployeeSelectedEvent), EventConstructorArgs = "_selectedItem.Id")]
        [Property]
        private NavigationItemViewModel? _selectedItem;

        partial void OnInitialize()
        {
            EventAggregator.RegisterSubscriber(this);
        }

        public void Load()
        {
            Items.Clear();
            foreach (var employee in EmployeeDataProvider.GetAll())
            {
                Items.Add(new NavigationItemViewModel { Id = employee.Id, FirstName = employee.FirstName });
            }
        }

        public void OnEvent(EmployeeSavedEvent eventData)
        {
            var item = Items.SingleOrDefault(x => x.Id == eventData.EmployeeId);
            if (item == null)
            {
                Items.Add(new NavigationItemViewModel { Id = eventData.EmployeeId, FirstName = eventData.FirstName });
            }
            else
            {
                item.FirstName = eventData.FirstName;
            }
        }

        public ObservableCollection<NavigationItemViewModel> Items { get; } = new();
    }

    [ViewModel]
    public partial class NavigationItemViewModel
    {
        [Property] int _id;
        [Property] string _firstName;
    }
}
