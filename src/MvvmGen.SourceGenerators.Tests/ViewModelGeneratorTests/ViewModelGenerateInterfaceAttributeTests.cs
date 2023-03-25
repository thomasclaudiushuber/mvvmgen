﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class ViewModelGenerateInterfaceAttributeTests : ViewModelGeneratorTestsBase
    {
        [Fact]
        public void GenerateInterfaceWithMembers()
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  [ViewModelGenerateInterface]
  [ViewModel]
  public partial class EmployeeViewModel
  {{
    [Property] string _firstName;

    public string LastName {{ get; set; }}

    public string FullName => FirstName + "" "" + LastName;

    public void CustomMethod() {{ }}

    public bool CustomMethodWithParameters(string name, int age) {{ }}
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase, IEmployeeViewModel
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public string FirstName
        {{
            get => _firstName;
            set
            {{
                if (_firstName != value)
                {{
                    _firstName = value;
                    OnPropertyChanged(""FirstName"");
                }}
            }}
        }}
    }}

    public interface IEmployeeViewModel
    {{
        string FirstName {{ get; set; }}
        string LastName {{ get; set; }}
        string FullName {{ get; }}
        void CustomMethod();
        bool CustomMethodWithParameters(string name, int age);
    }}
}}
");
        }

        [Fact]
        public void GenerateInterfaceWithMembersFromModel()
        {
            ShouldGenerateExpectedCode(
    $@"using MvvmGen;

namespace MyCode
{{
    public class Employee
    {{
        public string FirstName {{ get; set; }}
        public bool IsDeveloper {{ get; set; }}
    }}

    [ViewModelGenerateInterface]
    [ViewModel(typeof(Employee))]
    public partial class EmployeeViewModel
    {{
    }}
}}",
    $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase, IEmployeeViewModel
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public string FirstName
        {{
            get => Model.FirstName;
            set
            {{
                if (Model.FirstName != value)
                {{
                    Model.FirstName = value;
                    OnPropertyChanged(""FirstName"");
                }}
            }}
        }}

        public bool IsDeveloper
        {{
            get => Model.IsDeveloper;
            set
            {{
                if (Model.IsDeveloper != value)
                {{
                    Model.IsDeveloper = value;
                    OnPropertyChanged(""IsDeveloper"");
                }}
            }}
        }}

        protected MyCode.Employee Model {{ get; set; }}
    }}

    public interface IEmployeeViewModel
    {{
        string FirstName {{ get; set; }}
        bool IsDeveloper {{ get; set; }}
    }}
}}
");
        }

        [Fact]
        public void GenerateInterfaceWithCustomName()
        {
            ShouldGenerateExpectedCode(
    $@"using MvvmGen;

namespace MyCode
{{
    [ViewModelGenerateInterface(InterfaceName=""ICustomInterfaceName"")]
    [ViewModel]
    public partial class EmployeeViewModel
    {{
    }}
}}",
    $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase, ICustomInterfaceName
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();
    }}

    public interface ICustomInterfaceName
    {{
    }}
}}
");
        }

        [InlineData("", " : global::MvvmGen.ViewModels.ViewModelBase, IEmployeeViewModel")]
        [InlineData(" : MvvmGen.ViewModels.ViewModelBase", " : IEmployeeViewModel")]
        [Theory]
        public void ViewModelShouldExtendInterface(string inputExtendString, string expectedGeneratedExtendString)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  [ViewModelGenerateInterface]
  [ViewModel]
  public partial class EmployeeViewModel{inputExtendString}
  {{
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel{expectedGeneratedExtendString}
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();
    }}

    public interface IEmployeeViewModel
    {{
    }}
}}
");
        }
    }
}