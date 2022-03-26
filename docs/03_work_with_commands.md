# Work with Commands

Commands are used in MVVM to execute logic in your ViewModel. In this article you learn everything about creating commands:
1. [Create a Command](#create-a-command)
2. [Add Can-Execute logic](#add-can-execute-logic)
3. [Invalidate Commands](#invalidate-commands)
4. [Define the Command Property Name](#define-the-command-property-name)
5. [Use Command Parameters](#use-command-parameters)
6. [Call Async Methods](#call-async-methods)

## Create a Command

To create a command in your ViewModel, you decorate a method with the `Command` attribute like below.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command]
  private void Save() { }
}
```
In the generated partial class you find now a `SaveCommand` property of type `DelegateCommand`,
like you see it in the next code snippet. The `DelegateCommand` class is MvvmGen's `ICommand` implementation.
Note that from the constructor, the generated `InitializedCommands` method is called that initializes 
the `SaveCommand` property with a new `DelegateCommand` instance.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel()
  {
    this.InitializeCommands();
    this.OnInitialize();
  }

  partial void OnInitialize();

  private void InitializeCommands()
  {
    SaveCommand = new DelegateCommand(_ => Save());
  }

  public DelegateCommand SaveCommand { get; private set; }
}
```

## Add Can-Execute Logic

If you need a method for the can-execute logic, you specify it like below with the `Command` attribute’s
`CanExecuteMethod` property.

```csharp
[Command(CanExecuteMethod = nameof(CanSave))]
private void Save() { }

private bool CanSave()
{
  return true;
}
```

## Invalidate Commands

Look at the following `CanSave` method. It depends on the properties `FirstName` and `LastName`.
That means the `SaveCommand` should raise its `CanExecuteChanged` event when the properties
`FirstName` and `LastName` were changed.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private string _lastName;

  [Command(CanExecuteMethod = nameof(CanSave))]
  private void Save() { }

  private bool CanSave()
  {
    return !string.IsNullOrEmpty(_firstName)
      && !string.IsNullOrEmpty(_lastName);
  }
}
```

To raise the `CanExecuteChanged` event for a command when a property changes, you use the `CommandInvalidate` attribute like below:

```csharp
[CommandInvalidate(nameof(FirstName))]
[CommandInvalidate(nameof(LastName))]
private bool CanSave()
{
  return !string.IsNullOrEmpty(_firstName)
    && !string.IsNullOrEmpty(_lastName);
}
```
Instead of using separate `CommandInvalidate` attributes, you can also use a single attribute with multiple property names:

```csharp
[CommandInvalidate(nameof(FirstName),nameof(LastName))]
private bool CanSave()
{
  return !string.IsNullOrEmpty(_firstName)
    && !string.IsNullOrEmpty(_lastName);
}
```
You also have the choice if you want to set the `CommandInvalidate` attribute on the execute or on the can-execute method,
in this example here on the `Save` method or on the `CanSave` method. So, setting the attribute like below on the `Save`
method is fine too:

```csharp
[CommandInvalidate(nameof(FirstName), nameof(LastName))]
[Command(CanExecuteMethod = nameof(CanSave))]
private void Save() { }

private bool CanSave()
{
  return !string.IsNullOrEmpty(_firstName)
    && !string.IsNullOrEmpty(_lastName);
}
```

Thomas, what do you prefer? Thomas: _"I prefer it on the `CanSave` method like in the following ViewModel"_:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private string _lastName;


  [Command(CanExecuteMethod = nameof(CanSave))]
  private void Save() { }

  [CommandInvalidate(nameof(FirstName), nameof(LastName))]
  private bool CanSave()
  {
    return !string.IsNullOrEmpty(_firstName)
      && !string.IsNullOrEmpty(_lastName);
  }
}
```

Now, with the `CommandInvalidate` attribute in place, the generated partial class for the ViewModel above looks like below.
Note the added override of the `InvalidateCommands` method of the `ViewModelBase` class. It calls the `RaiseCanExecuteChanged`
method of the `SaveCommand` when the properties `FirstName` or `LastName` were changed.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel()
  {
    this.InitializeCommands();
    this.OnInitialize();
  }

  partial void OnInitialize();

  private void InitializeCommands()
  {
    SaveCommand = new DelegateCommand(_ => Save(), _ => CanSave());
  }

  public DelegateCommand SaveCommand { get; private set; }

  public string FirstName
  {
    get => _firstName;
    set
    {
      if (_firstName != value)
      {
        _firstName = value;
        OnPropertyChanged("FirstName");
      }
    }
  }

  public string LastName
  {
    get => _lastName;
    set
    {
      if (_lastName != value)
      {
        _lastName = value;
        OnPropertyChanged("LastName");
      }
    }
  }

  protected override void InvalidateCommands(string? propertyName)
  {
    base.InvalidateCommands(propertyName);
    if (propertyName == "FirstName")
    {
      SaveCommand.RaiseCanExecuteChanged();
    }
    else if (propertyName == "LastName")
    {
      SaveCommand.RaiseCanExecuteChanged();
    }
  }
}
```

## Define the Command Property Name

The following `Command` attribute generates a `SaveCommand` property. So, the generated command property name
is always the method name with a Command suffix.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command]
  private void Save() { }
}
```

If you want to define another property name for your command, you can use the `PropertyName` property
of the `Command` attribute.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command(PropertyName ="SaveItAllCommand")]
  private void Save() { }
}
```


The generated code code for the ViewModel above looks like below. Note the `SaveItAllCommand` property.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel()
  {
    this.InitializeCommands();
    this.OnInitialize();
  }

  partial void OnInitialize();

  private void InitializeCommands()
  {
    SaveItAllCommand = new DelegateCommand(_ => Save());
  }

  public DelegateCommand SaveItAllCommand { get; private set; }
}
```

## Use Command Parameters

If the View sends a parameter to your command, you can optionally define that parameter. It must be of type `object`:

```csharp
[Command]
private void Save(object obj) { }
```

When you define it on your execute method, it doesn't mean that you have to define it on your can-execute method:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command(CanExecuteMethod =nameof(CanSave))]
  private void Save(object obj) { }

  private bool CanSave() { return true;  }
}
```
Also the other way, defining it on the can-execute method, but not on the execute method works:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command(CanExecuteMethod =nameof(CanSave))]
  private void Save() { }

  private bool CanSave(object obj) { return true;  }
}
```
And of course, you can also define it on both methods:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Command(CanExecuteMethod =nameof(CanSave))]
  private void Save(object obj) { }

  private bool CanSave(object obj) { return true;  }
}
```

## Call Async Methods

Sometimes you might want to use an async execute handler. You can just do this like this:

```csharp
[Command]
private async void Save()
{
  await Task.Delay(2000);
}
```

Alternatively, you can also return a `Task` from that `Save` method:

```csharp
[Command]
private async Task Save()
{
  await Task.Delay(2000);
}
```

But in any case, you have to consider a few things with asynchronous methods. 

1. You should put your code into a try/catch block
to catch errors

```csharp
[Command]
private async void Save()
{
  try
  {
    await Task.Delay(2000);
    throw new Exception("Error");
  }
  catch (Exception ex)
  {
    // Do something in your ViewModel to tell the user that something went wrong
  }
}
```

2. Your method can be executed multiple times

To avoid this, you can define a simple `IsExecuting` property in your ViewModel

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] bool _isExecuting;

  [Command(CanExecuteMethod = nameof(CanExecute))]
  private async void Save()
  {
    IsExecuting = true;
    try
    {
      await Task.Delay(2000);
      throw new Exception("Error");
    }
    catch (Exception ex)
    {
      // Do something in your ViewModel
    }
    finally
    {
      IsExecuting = false;
    }
  }

  [CommandInvalidate(nameof(IsExecuting))]
  private bool CanExecute() => !IsExecuting;
} 
```

Note: Several developers implement an AsyncCommand to encapsulate that is-executing-logic. But MvvmGen thinks that 
this is-executing-logic is something that belongs into a ViewModel, and not in a command.
The execute method of a Command is pretty much like an event, which is fire and forget.

Next, let's look at how to [inject services into your ViewModel](04_inject_services.md).