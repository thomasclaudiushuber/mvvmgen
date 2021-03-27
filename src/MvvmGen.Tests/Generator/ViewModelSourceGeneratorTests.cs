using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
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
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    [Fact]
    public void GenerateWithoutINotifyPropertyChangedOnInput()
    {
      ShouldGenerateExpectedCode(
@"namespace MyCode
{   
    [ViewModel]
    public partial class EmployeeViewModel
    {
    }
}",
@"using System.ComponentModel;
using MvvmGen.Core;

namespace MyCode
{
  public partial class EmployeeViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void Initialize()
    {
    }
  }
}
");
    }

    [Fact]
    public void GenerateWithCommandProperty()
    {
      ShouldGenerateExpectedCode(
@"namespace MyCode
{   
    [ViewModel]
    public partial class EmployeeViewModel
    {
        [Command]public void SaveAll() { }
    }
}",
@"using System.ComponentModel;
using MvvmGen.Core;

namespace MyCode
{
  public partial class EmployeeViewModel : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    public void Initialize()
    {
      SaveAllCommand = new(SaveAll);
    }

    public DelegateCommand SaveAllCommand { get; private set; }
  }
}
");
    }
  }
}
