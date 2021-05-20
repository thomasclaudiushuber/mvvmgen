# Generate a ViewModel Factory

Sometimes you need to create a random number of ViewModel instances inside of 
another ViewModel. A typical case is when you have for example a `MainViewModel`
that has an `ObservableCollection<EmployeeViewModel>`. When a user selects 
an employee in the navigation, the `MainViewModel` creates a new 
`EmployeeViewModel` instance, adds it to its own collection, and that collection 
could be bound in the view for example by a TabControl, so that the selected 
employee shows up in a new tab page.

But now, with dependency injection in place, the creation of an `EmployeeViewModel`
shouldn’t be done with a hard-coded constructor call in the `MainViewModel`.
If you do that, you always have to adjust that constructor call when you change 
the constructor parameters of the `EmployeeViewModel`. To solve this, you should 
use a factory in the `MainViewModel` that allows you to create an `EmployeeViewModel`
without manually calling its constructor.

Some dependency injection frameworks support these factories with a `Func<T>`.
That means, you can inject a `Func<EmployeeViewModel>` into the `MainViewModel`,
and when you call that function, you get a new `EmployeeViewModel` instance, 
and the dependency injection framework calls the constructor for you and 
it passes all the necessary constructor parameters to the `EmployeeViewModel`
constructor. Now, the problem is that not all dependency injection frameworks 
support factories via the `Func<T>` delegate. And for these frameworks, 
you should implement a factory that can create a ViewModel for you. 

MvvmGen has an interface for such a ViewModel factory. As you can see in 
the following code snippet, the generic `IViewModelFactory<T>` interface 
has a parameterless `Create` method to create a ViewModel of type `T`.

```csharp
public interface IViewModelFactory<out T> where T : ViewModelBase
{
  T Create();
}
```

Of course, MvvmGen wouldn’t be MvvmGen if you would have to implement the 
`IViewModelFactory<T>` interface on your own. With MvvmGen you use the 
`ViewModelGenerateFactory` attribute to generate an implementation of 
`IViewModelFactory<T>`. You set that attribute on a ViewModel like you see 
it in the following code snippet:

```csharp
[Inject(typeof(IEmployeeDataProvider))]
[Inject(typeof(IEventAggregator))]
[ViewModelGenerateFactory]
[ViewModel(typeof(Employee))]
public partial class EmployeeViewModel
{
    ...
}
```

The attribute adds at the bottom of the generated _EmployeeViewModel.g.cs_ file the 
following code. As you can see, an `IEmployeeViewModelFactory` interface is created,
and that interface inherits from `IViewModelFactory<EmployeeViewModel>`.
Then there is an `EmployeeViewModelFactory` class that implements the 
`IEmployeeViewModelFactory` interface, and its `Create` method returns a new 
`EmployeeViewModel` instance by calling the constructor. 
The necessary constructor parameters, in this case the `IEventAggregator`
and the `IEmployeeDataProvider` are passed to the `EmployeeViewModel` constructor 
by the factory.

```csharp
public interface IEmployeeViewModelFactory : IViewModelFactory<EmployeeViewModel> { }

public class EmployeeViewModelFactory : IEmployeeViewModelFactory
{
  public EmployeeViewModelFactory(MvvmGen.Events.IEventAggregator eventAggregator, Sample.WpfApp.DataProvider.IEmployeeDataProvider employeeDataProvider)
  {
    this.EventAggregator = eventAggregator;
    this.EmployeeDataProvider = employeeDataProvider;
  }

  protected MvvmGen.Events.IEventAggregator EventAggregator { get; private set; }

  protected Sample.WpfApp.DataProvider.IEmployeeDataProvider EmployeeDataProvider { get; private set; }

  public EmployeeViewModel Create() => new EmployeeViewModel(EventAggregator, EmployeeDataProvider);
}
```

This means now that you can inject that factory into another ViewModel to create 
instances of the `EmployeeViewModel`. Look at the following snippet. 
The `IEmployeeViewModelFactory` is injected, and in the `MainViewModel`
its `Create` method is called to create an `EmployeeViewModel` instance.

```csharp
[Inject(typeof(IEmployeeViewModelFactory))]
[Inject(typeof(NavigationViewModel), PropertyAccessModifier = AccessModifier.Public)]
[Inject(typeof(IEventAggregator))]
[ViewModel]
public partial class MainViewModel : IEventSubscriber<EmployeeNavigationSelectedEvent>
{
  public void OnEvent(EmployeeNavigationSelectedEvent eventData)
  {
    var employeeViewModel = EmployeeViewModels
      .SingleOrDefault(x => x.Id == eventData.EmployeeId);
    if (employeeViewModel is null)
    {
      employeeViewModel = EmployeeViewModelFactory.Create();
      employeeViewModel.Load(eventData.EmployeeId);
      EmployeeViewModels.Add(employeeViewModel);
    }

    SelectedEmployee = employeeViewModel;
  }
  ...
}
```

Now, with the factory in place, it means that you can inject for example 
another service into the `EmployeeViewModel` by using MvvmGen’s `Inject` attribute,
that one generates a new constructor parameter in the `EmployeeViewModel`,
and it also updates the generated `EmployeeViewModelFactory` to create the 
`EmployeeViewModel` with that new constructor parameter, and so, in the end, 
everything just works with your factory when you inject other services, 
as everything gets generated on-the-fly in the compilation process.

A ViewModel factory like you saw it here in this article is actually used 
in the EmployeeManager application that you find in the [MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples).

Next, let's look at how to [set up dependency injection](07_set_up_dependency_injection.md).