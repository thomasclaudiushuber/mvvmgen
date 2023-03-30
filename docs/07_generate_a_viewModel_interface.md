# Generate a ViewModel Interface

To support some unit testing scenarios, it can be necessary to generate an interface for your viewModel. 
You can do this manually, or you can use MvvmGen's `ViewModelGenerateInterface` attribute.

This attribute will generate an interface for your ViewModel class that you decorated already 
with the `ViewModel` attribute. The code snippet below shows a ViewModel that uses the attribute.

```csharp
[ViewModelGenerateInterface]
[ViewModel]
public partial class EmployeeViewModel
{
  [Property] private string _firstName;
}
```

In the generated code below you can see that an `IEmployeeViewModel` interface is created
and the generated `EmployeeViewModel` class implements this interface.

```csharp
partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase, IEmployeeViewModel
{
     ...
}

public interface IEmployeeViewModel
{
    string FirstName { get; set; }
}
```

If you want, you can specify the name of the interface by using the attribute's `InterfaceName`
property like this: `[ViewModelGenerateInterface(InterfaceName="ICustomName")]`.

## Interfaces and Factories

When you also generate a factory for your ViewModel, the factory will automatically use
the generated interface as a return type for its `Create` method. Let's take the ViewModel
that you see below as an example:

```csharp
[ViewModelGenerateInterface]
[ViewModelGenerateFactory]
[ViewModel]
public partial class EmployeeViewModel
{
    [Property] private string _firstName;
}
```

Below you can see the generated code. Note that the factory uses now the generated
`IEmployeeViewModel` interface as a return type for its create method.

```csharp
partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase, IEmployeeViewModel
{
    ...
}

public interface IEmployeeViewModel
{
    string FirstName { get; set; }
}

public interface IEmployeeViewModelFactory : IViewModelFactory<IEmployeeViewModel> { }

public class EmployeeViewModelFactory : IEmployeeViewModelFactory
{
    public EmployeeViewModelFactory()
    {
    }

    public IEmployeeViewModel Create() => new EmployeeViewModel();
}
```

In case you want to generate an interface but still want a concrete return type from your factory,
you can use the factory attribute's `ReturnType` property like you see it in the code snippet below:

```csharp
[ViewModelGenerateInterface]
[ViewModelGenerateFactory(ReturnType = typeof(EmployeeViewModel))]
[ViewModel]
public partial class EmployeeViewModel
{
    [Property] private string _firstName;
}
```

Now you learned about generating interfaces for ViewModels. If you want to learn how interfaces and factories are used,
take a look at the EmployeeManager application that you find in the [MvvmGen Samples repository](https://github.com/thomasclaudiushuber/mvvmgen-samples).

Next, let's look at how to [set up dependency injection](08_set_up_dependency_injection.md).
