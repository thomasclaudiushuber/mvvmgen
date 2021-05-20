# Communicate between ViewModels with Mvvmgen's EventAggregator

In this article you learn how to use MvvmGen's `EventAggregator` to communicate between ViewModels.

1. [Define an Event](#define-an-event)
2. [Publish an Event](#publish-an-event)
3. [Subscribe to an Event](#subscribe-to-an-event)
4. [Publish an Event from a Property Setter](#publish-an-event-from-a-property-setter)

## Define an Event

An event in MvvmGen can be anything, and the type identifies the event.
Typically, with C# 9.0 or later, you define an event with a simple record like this:

```csharp
public record EmployeeSavedEvent(int id, string? FirstName);
```

## Publish an Event

To publish an event, you inject an `MvvmGen.Events.IEventAggregator` into your 
ViewModel with the `Inject` attribute. Then you use the generated
`EventAggregator` property like you see it in the `Save` method of this ViewModel:

```csharp
[Inject(typeof(IEventAggregator))]
[ViewModel(typeof(Employee))]
public partial class EmployeeViewModel
{
  [Property] private string _updateComment;

  partial void OnInitialize()
  {
    Model = new Employee
    {
      FirstName = "Thomas Claudius",
      IsDeveloper = true
    };
  }

  [Command(CanExecuteMethod = nameof(CanSave))]
  private void Save()
  {
    EventAggregator.Publish(new EmployeeSavedEvent(Model.Id, Model.FirstName));
  }

  [CommandInvalidate(nameof(FirstName))]
  private bool CanSave()
  {
    return !string.IsNullOrEmpty(FirstName);
  }
}
```

## Subscribe to an Event

To subscribe from a ViewModel to an event, you implement the generic 
`IEventSubscriber` interface that defines an `OnEvent` method:

```csharp
[ViewModel]
public partial class NavigationViewModel : IEventSubscriber<EmployeeSavedEvent>
{
  public void OnEvent(EmployeeSavedEvent eventData) { }
}
```

When you look at the generated code of this `NavigationViewModel` class,
you see that an `IEventAggregator` constructor parameter is created,
and in the constructor the `RegisterSubscriber` method is called 
on the `IEventAggregator` to register the ViewModel instance as an event subscriber:

```csharp
partial class NavigationViewModel : ViewModelBase
{
  public AnotherViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
  {
    eventAggregator.RegisterSubscriber(this);
    this.OnInitialize();
  }

  partial void OnInitialize();
}
```

This means that you just implement the `IEventSubscriber` interface,
and your `OnEvent` method is called when the event was published. 
There’s nothing more for you to do. If you need to subscribe to multiple events, 
you can implement the `IEventSubscriber` interface multiple times, 
or you use overloads of this interface that have multiple generic type parameters. 
If you need to publish events in addition, you use the `Inject` attribute 
to inject the `IEventAggregator`, so that you can call `EventAggregator.Publish`
in your code.

## Publish an Event from a Property Setter

If you want to publish an event when a property changes, you could use
the `PropertyCallMethod` attribute to do publish the event in a custom method.
But an easier way is the `PropertyPublishEvent` attribute. 
The following field has that attribute set. Beside the mandatory event type, 
you can specify optional `EventConstructorArgs` and an optional `PublishCondition`:

```csharp
[PropertyPublishEvent(typeof(EmployeeNavigationSelectedEvent),
  EventConstructorArgs = "value.Id",
  PublishCondition = "value is not null")]
[Property]
private NavigationItemViewModel? _selectedItem;
```

The generated property for the field above looks like this:

```csharp
public EmployeeManager.ViewModel.NavigationItemViewModel? SelectedItem
{
  get => _selectedItem;
  set
  {
    if (_selectedItem != value)
    {
      _selectedItem = value;
      OnPropertyChanged("SelectedItem");
      if (value is not null)
      {
        EventAggregator.Publish(new EmployeeManager.ViewModel.Events.EmployeeNavigationSelectedEvent(value.Id));
      }
    }
  }
}
```

Next, let's look at how to [generate a ViewModelFactory](06_generate_a_viewModel_factory.md).