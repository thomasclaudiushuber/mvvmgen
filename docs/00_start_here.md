# Welcome to the MvvmGen Docs

**MvvmGen** is a modern and lightweight MVVM library that is built 
on top of C# Source Generators. It is a "Source Generator First/Only" framework 
that contains everything that you need to build XAML applications with the popular 
Model-View-ViewModel-pattern (MVVM):
- A `ViewModelBase` class
- An `ICommand` implementation
- An EventAggregator to communicate between ViewModels
- Attributes like `ViewModel`, `Property`, `Command`
and `Inject` that you use to hook up MvvmGen’s C# Source Generator, the 
so-called `ViewModelGenerator`

Here in the docs you'll learn about the features of **MvvmGen**. Beside the docs,
please also check out [the MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples) 
that contains full applications built with WPF, WinUI, and MvvmGen.

You can read the docs on GitHub like a web page. 
At the end of each section you find a link to the next section. 
Here the separate articles to walk through:

1. [Generate a ViewModel](01_generate_a_viewModel.md)
2. [Create Properties](02_create_properties.md)
3. [Work with Commands](03_work_with_commands.md)
4. [Inject Services](04_inject_services.md)
5. [Communicate between ViewModels (EventAggregator)](05_communicate_between_viewModels.md)
6. [Generate a ViewModel Factory](06_generate_a_viewModel_factory.md)
7. [Generate a ViewModel Interface](07_generate_a_viewModel_interface.md)
8. [Set up Dependency Injection](08_set_up_dependency_injection.md)

