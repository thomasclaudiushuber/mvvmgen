// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Threading.Tasks;
using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class CommandAttributeTests : ViewModelGeneratorTestsBase
    {
        [Fact]
        public Task GenerateCommandProperty()
        {
            return VerifyGenerateCode(
                @"using MvvmGen;

namespace MyCode
{
  [ViewModel]
  public partial class EmployeeViewModel
  {
    [Command]public void SaveAll() { }
  }
}");
        }

        [InlineData("CanExecuteMethod=\"CanSaveAll\"")]
        [InlineData("CanExecuteMethod=nameof(CanSaveAll)")]
        [InlineData("nameof(CanSaveAll)")]
        [InlineData("\"CanSaveAll\"")]
        [Theory]
        public Task GenerateCommandPropertyWithCanExecuteMethod(string attributeArgument)
        {
            return VerifyGenerateCode(
                    $@"using MvvmGen;

namespace MyCode
{{
    [ViewModel]
    public partial class EmployeeViewModel
    {{
        [Command({attributeArgument})]
        public void SaveAll() {{ }}

        public bool CanSaveAll() => true;
    }}
}}")
                .UseParameters(attributeArgument);
        }

        [InlineData("CanExecuteMethod=nameof(CanSaveAll)")]
        [InlineData("CanExecuteMethod=\"CanSaveAll\"")]
        [InlineData("nameof(CanSaveAll)")]
        [InlineData("\"CanSaveAll\"")]
        [Theory]
        public Task GenerateCommandPropertyWithCanExecuteMethodAndCommandNameUsingNamedArgument(string canExecuteParameter)
        {
            return VerifyGenerateCode(
                $@"using MvvmGen;

namespace MyCode
{{
  [ViewModel]
  public partial class EmployeeViewModel
  {{
    [Command({canExecuteParameter}, PropertyName=""SuperCommand"")]
    public void SaveAll() {{ }}

    public bool CanSaveAll() => true;
  }}
}}")
                .UseParameters(canExecuteParameter);
        }

        [InlineData("async Task Save(object o)", "bool CanSave()")]
        [InlineData("async Task Save()", "bool CanSave()")]
        [InlineData("async void Save()", "bool CanSave()")]
        [InlineData("void Save()", "bool CanSave()")]
        [InlineData("void Save(object o)", "bool CanSave()")]
        [InlineData("void Save(object o)", "bool CanSave(object o)")]
        [InlineData("void Save()", "bool CanSave(object o)")]
        [Theory]
        public Task GenerateCommandWithParametersOnMethods(string executeMethod, string canExecuteMethod)
        {
            return VerifyGenerateCode(
                    $@"using MvvmGen;

namespace MyCode
{{
    [ViewModel]
    public partial class EmployeeViewModel
    {{
        [Command(CanExecuteMethod=nameof(CanSave))]
        public {executeMethod} {{ }}

        public {canExecuteMethod} => true;
    }}
}}")
                .UseParameters(executeMethod, canExecuteMethod);
        }
    }
}
