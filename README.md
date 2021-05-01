[![MvvmGen Build](https://github.com/thomasclaudiushuber/mvvmgen/actions/workflows/mvvmgen_build.yml/badge.svg)](https://github.com/thomasclaudiushuber/mvvmgen/actions/workflows/mvvmgen_build.yml)

# ⚡ MvvmGen 
## The Next Generation MVVM library – Say Goodbye to the Boilerplate
Hey there, welcome to the **MvvmGen** repository. MvvmGen is a next generation MVVM library that helps you to apply the popular Model-View-ViewModel-pattern (MVVM) in your XAML applications that you build with WPF, WinUI, Xamarin, or .NET MAUI.

MvvmGen uses C# Source Generators that are based on the Roslyn compiler to generate all the ViewModel boilerplate for you. 

Yes, creating a ViewModel is a lot of fun with **MvvmGen**! 🔥

## How does it work? 
Reference the NuGet package MvvmGen in your .NET application, and then you're ready to go. MvvmGen will register itself as a C# source generator in your project, and it will be your friend who writes the boilerplate for you.

## Generating a ViewModel class
To generate a ViewModel class, you just create a new class, you mark it as `partial`, and you put MvvmGen's `ViewModel` attribute on the class:

```csharp
using MvvmGen; 

[ViewModel]
public partial class EmployeeViewModel { }

```
The `ViewModel` attribute tells MvvmGen to generate another partial `EmployeeViewModel` class. Right now, it will be empty and look like this:
```
public partial class EmployeeViewModel 
{
}
```


Let's say you have this simple Model class:

```csharp
public class Employee
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
```
To create a ViewModel for this `Employee` class, you typically wrap the class and you re-create the properties with full setters, so that you can raise the `PropertyChanged` in the setters, as that event is necessary for the data bindings in the UI. Creating all these properties is something you have to do again and again and again when working with the MVVM pattern. But not with **MvvmGen**.

## Generating Model properties with MvvmGen

When you reference the MvvmGen NuGet package in your project, the source generator gets active when you add the `MvvmGen.ViewModel` attribute to a class like this:

```csharp
[ViewModel(typeof(Employee))]
public partial class EmployeeViewModel { }
```
This attribute will generate on the fly while you're typing another partial `EmployeeViewModel` class that gets compiled with your project and that looks like this:

```

