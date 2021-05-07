using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MvvmGen.Events;
using MvvmGen.ViewModels;
using Sample.WpfApp.DataProvider;
using Sample.WpfApp.ViewModel;

namespace Sample.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IEventAggregator, EventAggregator>();
            serviceCollection.AddTransient<MainWindow>();
            serviceCollection.AddTransient<MainViewModel>();
            serviceCollection.AddTransient<IEmployeeDataProvider, EmployeeDataProvider>();
            serviceCollection.AddTransient<NavigationViewModel>();
            serviceCollection.AddTransient<IEmployeeViewModelFactory, EmployeeViewModelFactory>();

            ServiceProvider = serviceCollection.BuildServiceProvider(true);

            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow?.Show();            
        }
    }
}
