

# ⚡ MvvmGen 

[![MvvmGen Build](https://github.com/thomasclaudiushuber/mvvmgen/actions/workflows/mvvmgen_build.yml/badge.svg)](https://github.com/thomasclaudiushuber/mvvmgen/actions/workflows/mvvmgen_build.yml)

## The Next Generation MVVM library – Say Goodbye to the Boilerplate

Hey there, welcome to the **MvvmGen** repository. MvvmGen is a lightweight and modern MVVM library (.NET Standard 2.0) that helps you to apply the popular Model-View-ViewModel-pattern (MVVM) in your XAML applications that you build with WPF, WinUI, Uno Platform, Xamarin Forms, or .NET MAUI.

MvvmGen contains everything you need to build XAML applications with the popular Model-View-ViewModel-pattern (MVVM):
- A `ViewModelBase` class
- An `ICommand` implementation
- An EventAggregator to communicate between ViewModels

**But the key part of MvvmGen** are its attributes that you use to hook up MvvmGen’s `ViewModelGenerator`. The `ViewModelGenerator` is a Roslyn-based C# Source Generator that generates the boilerplate for your ViewModel classes behind the scenes while you type your code in Visual Studio.

**MvvmGen** is a "Source Generators First/Only" framework, which means it is built on top of Roslyn-powered C# Source Generators. The generator is the heart of MvvmGen.

Now, you'll learn that creating a ViewModel is a lot of fun with **MvvmGen**! 🔥 (Also check out [the MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples) that contains full applications built with WPF, WinUI, and MvvmGen)


## How does it work? 
Reference the NuGet package [MvvmGen](https://www.nuget.org/packages/MvvmGen/) in your .NET application, and then you're ready to go. MvvmGen will register itself as a C# source generator in your project, and it will be your friend who writes the boilerplate for you.

## Generating a ViewModel class
To generate a ViewModel class, you create a new class, you mark it as `partial`, and you put MvvmGen's `ViewModel` attribute on the class:

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
The `ViewModel` attribute tells MvvmGen to generate another partial `EmployeeViewModel` class. Right now, it will be a class that looks like this:
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

But there are way more attributes in the `MvvmGen` namespace that you can use. They allow you to build a full ViewModel like this:
```csharp
using MvvmGen;
using MvvmGen.Events;

namespace MyWpfApp
{
  [Inject(typeof(IEventAggregator))]
  [ViewModel]
  public partial class EmployeeViewModel
  {
    [Property] private string _firstName;
    [Property] private string _lastName;

    [Command(CanExecuteMethod = nameof(CanSave))]
    private void Save()
    {
      EventAggregator.Publish(new EmployeeSavedEvent(FirstName, LastName));
    }

    [CommandInvalidate(nameof(FirstName))]
    private bool CanSave()
    {
      return !string.IsNullOrEmpty(FirstName);
    }
  }
}
```
For this ViewModel, MvvmGen will generate the following partial class definition for you
```csharp
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp
{
  partial class EmployeeViewModel : ViewModelBase
  {
    public EmployeeViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
    {
      this.EventAggregator = eventAggregator;
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
          SaveCommand.RaiseCanExecuteChanged();
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

    protected MvvmGen.Events.IEventAggregator EventAggregator { get; private set; }
  }
}
```

You can read more [in this blog post that introduces MvvmGen](https://www.thomasclaudiushuber.com/2021/05/12/introducing-the-mvvmgen-library). You find also a docs directory in this repository that explains the different attributes that you can use.

Please also check out [the MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples) that contains full applications built with WPF, WinUI, and MvvmGen.
