# Inject Services

In this article you learn everything about injecting services into your ViewModel:
1. [Inject a Service](#inject-a-service)
2. [Define a Custom Property Name](#define-a-custom-property-name)
3. [Set the Access Modifier](#set-the-access-modifier)

## Inject a Service

Usually you add constructor parameters to your ViewModel class, so that you can
pass dependencies to it. This keeps your ViewModel loosely coupled and testable. 
Also for this very typical scenario, MvvmGen has a feature. Let’s say you have 
an `EmployeeDataProvider` implementation like below that you use to save
an `Employee`. The Save method could save an Employee to a database, 
to a file or to any other place.

```csharp
public interface IEmployeeDataProvider
{
  void Save(Employee employee);
}

public class EmployeeDataProvider : IEmployeeDataProvider
{
  public void Save(Employee employee)
  {
    // TODO: Save employee to a file, a db, a REST API etc. 
  }
}
```

The ViewModel would depend on the `IEmployeeDataProvider` interface. 
So, in a unit test, you mock this interface with a test implementation 
that does not access the real database, and in your app, you use the real 
`EmployeeDataProvider`. 
Now, to inject a service like an `IEmployeeDataProvider` instance into 
your ViewModel, you use MvvmGen’s `Inject` attribute on your ViewModel class. 
With the `Inject` attribute, you define the service type that you want to inject,
in this case an `IEmployeeDataProvider`:

```csharp
[Inject(typeof(IEmployeeDataProvider))]
[ViewModel(ModelType = typeof(Employee))]
public partial class EmployeeViewModel
{ ... }
```
Now the generated class that you see in the next code snippet has an 
`IEmployeeDataProvider` constructor parameter. This constructor parameter 
is used to initialize a protected `EmployeeDataProvider` property that you 
can use in your code.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel(MyWpfApp.Data.IEmployeeDataProvider employeeDataProvider)
  {
    this.EmployeeDataProvider = employeeDataProvider;
    ...
  }
  ...

  protected MyWpfApp.Data.IEmployeeDataProvider EmployeeDataProvider { get; private set; }
}
```

Now, again, it’s important to understand that as soon as you have set the 
`[Inject(typeof(IEmployeeDataProvider))]` attribute on your ViewModel class,
the `EmployeeDataProvider` property is immediately available for you to use,
as the code generation happens on-the-fly while you type your code. 
That means, in your ViewModel, you can immediately use the `EmployeeDataProvider`
property for example in the `Save` method like you see it in the following 
code snippet:

```csharp
[Inject(typeof(IEmployeeDataProvider))]
[ViewModel(typeof(Employee))]
public partial class EmployeeViewModel
{
  [Property] private string _updateComment;

  partial void OnInitialize()
  {
    Model = new Employee
    {
      FirstName = "Thomas Claudius"
    };
  }

  [Command(CanExecuteMethod = nameof(CanSave))]
  private void Save()
  {
    EmployeeDataProvider.Save(Model);
  }

  [CommandInvalidate(nameof(FirstName))]
  private bool CanSave()
  {
    return !string.IsNullOrEmpty(FirstName);
  }
}
```

## Define a Custom Property Name

By default, MvvmGen generates a property for your service like you would expect it.
For an injected interface like `IEmployeeDataProvider`, a property with the name
`EmployeeDataProvider` is generated, as you saw in the preview code snippets.

If you want to use a different generated property name, you can use the `PropertyName`
property of the `Inject` attribute:

```csharp
[Inject(typeof(IEmployeeDataProvider),PropertyName ="EmpDataProv")]
[ViewModel]
public partial class EmployeeViewModel  { }
```

Instead of using the `PropertyName` property, you can also pass the property name 
directly to the constructor of the `Inject` attribute:

```csharp
[Inject(typeof(IEmployeeDataProvider), "EmpDataProv")]
[ViewModel]
public partial class EmployeeViewModel  { }
```

When you look now at the generated code, you can see that the generated property
has the name `EmpDataProv`, exactly like specified with the `Inject` attribute.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel(MyWpfApp.ViewModel.IEmployeeDataProvider empDataProv)
  {
    this.EmpDataProv = empDataProv;
    this.OnInitialize();
  }

  partial void OnInitialize();

  protected MyWpfApp.ViewModel.IEmployeeDataProvider EmpDataProv { get; private set; }
}
}
```

## Set the Access Modifier

The generated property for an injected service is by default `protected`.
If needed, you can specify a different access modifier by using the 
`PropertyAccessModifier` property of the `Inject` attribute:

```csharp
[Inject(typeof(IEmployeeDataProvider), 
  PropertyAccessModifier = AccessModifier.Public)]
[ViewModel]
public partial class EmployeeViewModel  { }
```

This generates the following ViewModel. Note that the `EmployeeDataProvider` property is now public.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel(MyWpfApp.ViewModel.IEmployeeDataProvider employeeDataProvider)
  {
    this.EmployeeDataProvider = employeeDataProvider;
    this.OnInitialize();
  }

  partial void OnInitialize();

  public MyWpfApp.ViewModel.IEmployeeDataProvider EmployeeDataProvider { get; private set; }
}
```

Next, let's look at how to [communicate between ViewModels](05_communicate_between_viewModels.md).