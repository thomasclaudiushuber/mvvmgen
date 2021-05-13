## Create a ViewModel

To create a ViewModel, you define a partial class in your code and you add the `ViewModel` attribute to it:

```csharp
using MvvmGen;

namespace MyWpfApp.ViewModel
{
  [ViewModel]
  public partial class EmployeeViewModel
  {
  }
}
```
The `ViewModel` attribute tells MvvmGen's `ViewModelGenerator` to generate in this case a partial `EmployeeViewModel` class that looks like this:

```csharp
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp.ViewModel
{
    partial class EmployeeViewModel : ViewModelBase
    {
        public EmployeeViewModel()
        {
            this.OnInitialize();
        }

        partial void OnInitialize();
    }
}
```

Next, you might want to [define some properties in your ViewModel](https://github.com/thomasclaudiushuber/mvvmgen/blob/main/docs/02_define_properties.md).
