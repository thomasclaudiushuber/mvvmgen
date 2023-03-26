# Set up Dependency Injection

MvvmGen does not depend on a specific dependency injection library. That means,
you can use your favorite library.

The EmployeeManager application from the [MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples)
uses the NuGet package `Microsoft.Extensions.DependencyInjection`. When you take a look
at the _App.xaml.cs_ file of the WPF project there, you can see the following code:


```csharp
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
    serviceCollection.AddTransient<IEmployeeDataProvider, EmployeeFileDataProvider>();
    serviceCollection.AddTransient<INavigationViewModel, NavigationViewModel>();
    serviceCollection.AddTransient<IEmployeeViewModelFactory, EmployeeViewModelFactory>();

    ServiceProvider = serviceCollection.BuildServiceProvider(true);

    var mainWindow = ServiceProvider.GetService<MainWindow>();
    mainWindow?.Show();
  }
}
```

The important part is that you register the `EventAggregator`
as a singleton. All ViewModels should get the same `EventAggregator` instance,
so that the ViewModel communication works.