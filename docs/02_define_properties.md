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
With this ViewModel, MvvmGen will generate the following partial class
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
Next, let's look at how you can customize the generated properties.
