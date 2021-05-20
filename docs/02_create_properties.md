# Create Properties

In this article you learn everything about creating properties:
1. [Define Properties](#define-properties)
2. [Call a Custom Method From a Setter](#call-a-custom-method-from-a-setter)
3. [Invalidate Readonly Properties](#invalidate-readonly-properties)
4. [Generate Properties From a Model](#generate-properties-from-a-model)

## Define Properties

To define properties, you set the `Property` attribute on fields that you define in your ViewModel class

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private bool _isDeveloper;
}
```
With this ViewModel, MvvmGen generates the following partial class
```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel()
  {
    this.OnInitialize();
  }

  partial void OnInitialize();

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

  public bool IsDeveloper
  {
    get => _isDeveloper;
    set
    {
      if (_isDeveloper != value)
      {
        _isDeveloper = value;
        OnPropertyChanged("IsDeveloper");
      }
    }
  }
}
```

## Call a Custom Method From a Setter
Sometimes you want to call a custom method from a property setter. 
To do this with MvvmGen, you use the `PropertyCallMethod` attribute
on a field that you annotated with the `Property` attribute.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [PropertyCallMethod(nameof(FirstNameChanged))]
  [Property]
  private string _firstName;

  private void FirstNameChanged() { }
}
```

The generated `FirstName` property in this ViewModel looks like below. 
As you can see, the `FirstNameChanged` method gets called in the setter.

```csharp
public string FirstName
{
  get => _firstName;
  set
  {
    if (_firstName != value)
    {
      _firstName = value;
      OnPropertyChanged("FirstName");
      FirstNameChanged();
    }
  }
}
```
Optionally you can also pass method arguments to your method.
To do this, you set the `MethodArgs` property of the `PropertyCallMethod` attribute.
The `MethodArgs` property is of type string, and its value is copied
one-to-one to the generated method call. Look at the following ViewModel,
where a NameChanged method has two parameters. That method should
be called when the `FirstName` property changes, but also when the
`LastName` property changes. So, the `PropertyCallMethod` attribute
is used with its `MethodArgs` property to define the two arguments.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [PropertyCallMethod(nameof(NameChanged),
    MethodArgs = "\"FirstName\",_firstName")]
  [Property]
  private string _firstName;

  [PropertyCallMethod(nameof(NameChanged),
    MethodArgs = "\"LastName\",_lastName")]
  [Property]
  private string _lastName;

  private void NameChanged(string propertyName, string value) { }
}
```
In the following snippet you see the two generated properties. 
Note how the string defined with the `MethodArgs` property is copied
one-to-one to the generated method call.
```csharp
public string FirstName
{
  get => _firstName;
  set
  {
    if (_firstName != value)
    {
      _firstName = value;
      OnPropertyChanged("FirstName");
      NameChanged("FirstName", _firstName);
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
      NameChanged("LastName", _lastName);
    }
  }
}
```

## Invalidate Readonly Properties

When you create readonly properties in your ViewModel that depend
on other properties, then you need to ensure that a `PropertyChanged` event
is raised for your property when these other properties changed. A typical
example is when you have the properties `FirstName` and `LastName`, and then
you also define a readonly `FullName` property in your ViewModel
like you see it in the code snippet below.

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private string _lastName;

  public string FullName => $"{FirstName} {LastName}";
}
```
The `FullName` property depends on the properties `FirstName` and `LastName`,
so the `PropertyChanged` event needs to be raised when but when
`FirstName` or `LastName` changes. But when you look at the generated code,
then you can see that this does not happen:

```csharp
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
```
To ensure that the `PropertyChanged` event is raised for the `FullName`
property in the setters of the properties `FirstName` and `LastName`,
you use the `PropertyInvalidate` attribute on your readonly property:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private string _lastName;

  [PropertyInvalidate(nameof(FirstName))]
  [PropertyInvalidate(nameof(LastName))]
  public string FullName => $"{FirstName} {LastName}";
}
```
Instead of using two `PropertyInvalidate` attributes for this case, you can
also use a single attribute if you prefer this:

```csharp
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
  [Property] private string _lastName;

  [PropertyInvalidate(nameof(FirstName), nameof(LastName))]
  public string FullName => $"{FirstName} {LastName}";
}
```
Now, with the `PropertyInvalidate` attribute in place, the generated code
looks like below. As you can see, now the `PropertyChanged` event is raised
for the `FullName` property in the setters of the properties 
`FirstName` and `LastName`:

```csharp
public string FirstName
{
  get => _firstName;
  set
  {
    if (_firstName != value)
    {
      _firstName = value;
      OnPropertyChanged("FirstName");
      OnPropertyChanged("FullName");
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
      OnPropertyChanged("FullName");
    }
  }
}
```

## Generate Properties From a Model
Sometimes you create a ViewModel for a specific model.
 For this scenario, you can use the `ViewModel` attribute
to generate the required model properties in your ViewModel.

Let's say that the following `Employee` class is your model:

```csharp
public class Employee
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
```
You can use that `Employee` class in a ViewModel by setting the `ModelType`
property of the `ViewModel` attribute:
```csharp
[ViewModel(ModelType = typeof(Employee))]
public partial class EmployeeViewModel { }
```
Alternatively to the `ModelType` property, you can also use the `ViewModel` attribute's constructor:
```csharp
[ViewModel(typeof(Employee))]
public partial class EmployeeViewModel { }
```
For this ViewModel, MvvmGen generates the following partial class.
At the bottom you find a protected `Model` property of type `Employee`.
Then there are the properties `FirstName` and `LastName` that
wrap the corresponding properties of the `Employee` model.

```csharp
partial class EmployeeViewModel : ViewModelBase
{
  public EmployeeViewModel()
  {
    this.OnInitialize();
  }

  partial void OnInitialize();

  public string? FirstName
  {
    get => Model.FirstName;
    set
    {
      if (Model.FirstName != value)
      {
        Model.FirstName = value;
        OnPropertyChanged("FirstName");
      }
    }
  }

  public string? LastName
  {
    get => Model.LastName;
    set
    {
      if (Model.LastName != value)
      {
        Model.LastName = value;
        OnPropertyChanged("LastName");
      }
    }
  }

  protected MyWpfApp.ViewModel.Employee Model { get; set; }
}
```

So far to creating properties. Next, let's look at how to
[work with commands](03_work_with_commands.md).