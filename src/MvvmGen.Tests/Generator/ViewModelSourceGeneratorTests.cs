using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MvvmGen.SourceGenerator;
using System.ComponentModel;
using System.Reflection;
using Xunit;

namespace MvvmGen.Generator
{
  public class ViewModelSourceGeneratorTests
  {
    private void ShouldGenerateExpectedCode(string inputCode, string expectedGeneratedCode)
    {
      Compilation inputCompilation = CreateCompilation(inputCode);

      ViewModelSourceGenerator generator = new();

      GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

      driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);

      GeneratorDriverRunResult runResult = driver.GetRunResult();

      Assert.Single(runResult.GeneratedTrees);
      Assert.True(runResult.Diagnostics.IsEmpty);

      GeneratorRunResult generatorResult = runResult.Results[0];
      Assert.Equal(generator, generatorResult.Generator);
      Assert.True(generatorResult.Diagnostics.IsEmpty);
      Assert.Single(generatorResult.GeneratedSources);
      Assert.Null(generatorResult.Exception);

      Assert.Equal(expectedGeneratedCode, generatorResult.GeneratedSources[0].SourceText.ToString());
    }

    private static Compilation CreateCompilation(string source)
            => CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { 
                  MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),     
                  MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).GetTypeInfo().Assembly.Location),
                  MetadataReference.CreateFromFile(typeof(ViewModelAttribute).GetTypeInfo().Assembly.Location)},
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    [Fact]
    public void GenerateWithFullQualifiedAttributeName()
    {
      ShouldGenerateExpectedCode(
@"namespace MyCode
{   
  [MvvmGen.ViewModel]
  public partial class EmployeeViewModel
  {
  }
}",
@"using System.ComponentModel;
using MvvmGen.Commands;
using MvvmGen.ViewModels;

namespace MyCode
{
  public partial class EmployeeViewModel : ViewModelBase
  {
    public void Initialize()
    {
    }
  }
}
");
    }


    [Fact]
    public void GenerateViewModelBase()
    {
      ShouldGenerateExpectedCode(
@"using MvvmGen;

namespace MyCode
{   
  [ViewModel]
  public partial class EmployeeViewModel
  {
  }
}",
@"using System.ComponentModel;
using MvvmGen.Commands;
using MvvmGen.ViewModels;

namespace MyCode
{
  public partial class EmployeeViewModel : ViewModelBase
  {
    public void Initialize()
    {
    }
  }
}
");
    }

    [Fact]
    public void GenerateViewModelBaseNotIfClassInheritsFromOtherClass()
    {
     ShouldGenerateExpectedCode(
@"using System.ComponentModel;
using MvvmGen;

namespace MyCode
{   
  public class CustomViewModelBase
  {
    protected void OnPropertyChanged(string propertyName) { }
  }

  [ViewModel]
  public partial class EmployeeViewModel : CustomViewModelBase
  {
  }
}",
@"using System.ComponentModel;
using MvvmGen.Commands;
using MvvmGen.ViewModels;

namespace MyCode
{
  public partial class EmployeeViewModel
  {
    public void Initialize()
    {
    }
  }
}
");
    }

    [Fact]
    public void GenerateCommandProperty()
    {
      ShouldGenerateExpectedCode(
@"using MvvmGen;

namespace MyCode
{   
  [ViewModel]
  public partial class EmployeeViewModel
  {
    [Command]public void SaveAll() { }
  }
}",
@"using System.ComponentModel;
using MvvmGen.Commands;
using MvvmGen.ViewModels;

namespace MyCode
{
  public partial class EmployeeViewModel : ViewModelBase
  {
    public void Initialize()
    {
      SaveAllCommand = new(SaveAll);
    }

    public DelegateCommand SaveAllCommand { get; private set; }
  }
}
");
    }

    [Fact]
    public void GenerateCommandPropertyWithCanExecuteMethod()
    {
      ShouldGenerateExpectedCode(
@"using MvvmGen;

namespace MyCode
{   
  [ViewModel]
  public partial class EmployeeViewModel
  {
    [Command(nameof(CanSaveAll))]
    public void SaveAll() { }

    public bool CanSaveAll() => true;
  }
}",
@"using System.ComponentModel;
using MvvmGen.Commands;
using MvvmGen.ViewModels;

namespace MyCode
{
  public partial class EmployeeViewModel : ViewModelBase
  {
    public void Initialize()
    {
      SaveAllCommand = new(SaveAll, CanSaveAll);
    }

    public DelegateCommand SaveAllCommand { get; private set; }
  }
}
");
    }
  }
}
