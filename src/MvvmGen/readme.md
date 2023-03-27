## ⚡ MvvmGen - a Lightweight and Modern MVVM library 
**MvvmGen** is a lightweight and modern library that helps you 
to apply the popular Model-View-ViewModel-pattern (MVVM) in your XAML applications. MvvmGen works for your apps
that you build with WPF, WinUI, Xamarin.Forms, .NET MAUI, Uno Platform, AvaloniaUI, or any other .NET stack.

MvvmGen was created from ground up with a focus on **C# Source Generators**. 
With MvvmGen, a lot of your ViewModel code gets generated behind the scenes while you're typing code in your code editor.
This makes MvvmGen **the most productive MVVM library**.

## What's in the Package?

MvvmGen consists of three parts:
- **Framework classes needed to apply the MVVM pattern**
  - A `ViewModelBase` class that implements `INotifyPropertyChanged`
  - A `DelegateCommand` class that implements `ICommand`
  - An `EventAggregator` class for ViewModel communication
- **Attributes to decorate your ViewModels**
  - The `[ViewModel]` attribute is the most popular attribute, as it marks a class as a ViewModel. 
    With this attribute set on a class, the source generator knows there's something to generate
  - There are several other attributes, like `[Property]`, `[Command]`, or `[Inject]`, that tell
    the source generator what it should generate
- **A modern C# source generator**
  - This is your best friend who generates the ViewModel boilerplate for you behind the scenes

## Getting Started

To get started with MvvmGen, install either the NuGet package *MvvmGen* or *MvvmGen.PureCodeGeneration*. 

```
dotnet add package MvvmGen
```
```
dotnet add package MvvmGen.PureCodeGeneration
```

From a usage perspective, both packages work exactly the same. The difference is in the code generation.
- ***MvvmGen*** package - ViewModels are generated, but attributes and framework classes come from a referenced *MvvmGen.dll* that is part of the *MvvmGen* NuGet package.
- ***MvvmGen.PureCodeGeneration*** package - Not only ViewModels are generated, but also attributes and framework classes. This means 
  that your compiled assembly does not have a dependency on an *MvvmGen.dll*, as all the MvvmGen-specific code is generated at compile time.

If you need help for a decision: The ***MvvmGen*** package is the more popular package. But as the API of both packages is exactly 
the same, you can swap one for the other at any time.

## Creating a ViewModel

To create a ViewModel with MvvmGen, you create a partial class that you decorate with the `ViewModel` attribute:

```
using MvvmGen;

namespace MyWpfApp.ViewModel
{
    [ViewModel]
    public partial class EmployeeViewModel { }
}
```

Because of the `ViewModel` attribute, MvvmGen's source generator will generate another partial class behind the scenes like the one below:

```
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp.ViewModel
{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {
        public EmployeeViewModel()
        {
            this.OnInitialize();
        }

        partial void OnInitialize();
    }
}
```

Next, let's generate some properties.

## Generate Properties

To generate properties, you decorate your private fields with MvvmGen's `Property` attribute:

```
using MvvmGen;

namespace MyWpfApp.ViewModel
{
    [ViewModel]
    public partial class EmployeeViewModel
    {
        [Property] string _firstName;
        [Property] string _lastName;
    }
}
```

Below you can see the generated code. It contains the two properties `FirstName` and `LastName`.
In the setters of these properties the `PropertyChanged` event is raised by calling
the `OnPropertyChanged` method that is defined in the `ViewModelBase` class.
This event notifies data bindings in the user interface about property changes.

```
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp.ViewModel
{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
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
    }
}
```

So, as you can see in the code snippet above, all the property boilerplate is generated for you.
Now, let's also generate a command.

## Generate Commands

To generate a command, you decorate a method with the `Command` attribute like you see it
in the code snippet below. If you have some can-execute logic, you set the attribute's optional
`CanExecuteMethod` property. In the code snippet below it's set to the `CanSave` method.
On this `CanSave` method there's a `CommandInvalidate` attribute that ensures in this case 
that the command's `CanExecuteChanged` event is raised everytime the ViewModel's `FirstName` property
was changed.

```
using MvvmGen;

namespace MyWpfApp.ViewModel
{
    [ViewModel]
    public partial class EmployeeViewModel
    {
        [Property] string _firstName;
        [Property] string _lastName;

        [Command(CanExecuteMethod = nameof(CanSave))]
        private void Save() { }

        [CommandInvalidate(nameof(FirstName))]
        private bool CanSave()
        {
            return !string.IsNullOrEmpty(FirstName);
        }
    }
}
```

Below you see the generated code that contains now a `SaveCommand` property that gets initialized
with an instance of MvvmGen's `DelegateCommand` class. The `DelegateCommand` instance points to the
methods `Save` and `CanSave` defined in your code. There's also an `InvalidateCommands` method
that raises the command's `CanExecuteChanged` event if the `FirstName` property was changed.

```
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp.ViewModel
{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
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

        public string FirstName { ... }

        public string LastName { ... }

        protected override void InvalidateCommands(string? propertyName)
        {
            base.InvalidateCommands(propertyName);
            if (propertyName == "FirstName")
            {
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
```

Next, let's also inject and use a service.

## Inject Services

With MvvmGen's `Inject` attribute you can inject one or more services into your ViewModel. In the code snippet below
an `IEventAggregator` is injected. In the `Save` method the injected service is used to publish an event.

```
using MvvmGen;
using MvvmGen.Events;

namespace MyWpfApp.ViewModel
{
    public record EmployeeSavedEvent(string FirstName, string LastName);

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

Below you can see the generated code. As you can see, there's a new constructor parameter
of type `IEventAggregator`. The parameter is stored in an `EventAggregator` property that
you can use in your code like demonstrated in the `Save` method in the code snippet above.

```
using MvvmGen.Commands;
using MvvmGen.Events;
using MvvmGen.ViewModels;

namespace MyWpfApp.ViewModel
{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {
        public EmployeeViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
        {
            this.EventAggregator = eventAggregator;
            this.InitializeCommands();
            this.OnInitialize();
        }

        ...

        protected MvvmGen.Events.IEventAggregator EventAggregator { get; private set; }
    
        ...
    }
}
```

## Learning More

Now you learned how to use some basic features of the MvvmGen library to build and generate ViewModels.
If you want to learn more about MvvmGen:
- Take a look at the official docs, which are in the docs folder of the [MvvmGen repository](https://github.com/thomasclaudiushuber/mvvmgen/blob/main/docs/00_start_here.md)
- Browse the [MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples). It contains for example the very popular
  ***EmployeeManager*** application built with WPF and WinUI that has a tabbed user interface.

## Questions and Feedback

If you have any questions or feedback, you can always open an issue in the [MvvmGen repository](https://github.com/thomasclaudiushuber/mvvmgen)
or you can also contact me, the author and maintainer of MvvmGen, via
- [Twitter](https://www.twitter.com/thomasclaudiush)
- [LinkedIn](https://www.linkedin.com/in/thomasclaudiushuber)
- [Email](https://www.thomasclaudiushuber.com/contact)

Happy coding,  
Thomas Claudius Huber (https://www.thomasclaudiushuber.com)