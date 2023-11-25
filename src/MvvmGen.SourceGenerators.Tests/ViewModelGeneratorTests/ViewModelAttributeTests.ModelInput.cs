﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public partial class ViewModelAttributeTests : ViewModelGeneratorTestsBase
    {
        [InlineData("ModelType=typeof(Employee)")]
        [InlineData("typeof(Employee)")]
        [Theory]
        public void GenerateModelProperties(string attributeArgument)
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

    [ViewModel({attributeArgument})]
    public partial class EmployeeViewModel
    {{
    }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
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
}}
");
        }

        [InlineData("ModelType=typeof(Employee)")]
        [InlineData("typeof(Employee)")]
        [Theory]
        public void GenerateModelPropertiesFromBaseClasses(string attributeArgument)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
    public class Person
    {{
        public string FirstName {{ get; set; }}
    }}
    public class Employee : Person
    {{
        public bool IsDeveloper {{ get; set; }}
    }}

    [ViewModel({attributeArgument})]
    public partial class EmployeeViewModel
    {{
    }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
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
}}
");
        }

        [InlineData("ModelType=typeof(Employee)")]
        [InlineData("typeof(Employee)")]
        [Theory]
        public void GenerateModelPropertiesWhenReadOnlyPropsExist(string attributeArgument)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
    public class Employee
    {{
        public Employee (int id)
        {{
            Id = id;
        }}
        public int Id {{ get; }}
        public bool IsDeveloper {{ get; set; }}
    }}

    [ViewModel({attributeArgument})]
    public partial class EmployeeViewModel
    {{
    }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public int Id => Model.Id;

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
}}
");
        }

        [InlineData("CustomName", "ModelType=typeof(Employee), ModelPropertyName=\"CustomName\"")]
        [InlineData("CustomName", "typeof(Employee), ModelPropertyName=\"CustomName\"")]
        [InlineData("Model","ModelType=typeof(Employee))")]
        [InlineData("Model","typeof(Employee)")]
        [Theory]
        public void GenerateModelPropertyWithCustomName(string expectedModelPropertyName, string attributeArgument)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
    public class Employee
    {{
        public Employee (int id)
        {{
            Id = id;
        }}
        public int Id {{ get; }}
        public bool IsDeveloper {{ get; set; }}
    }}

    [ViewModel({attributeArgument})]
    public partial class EmployeeViewModel
    {{
    }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel()
        {{
            this.OnInitialize();
        }}

        partial void OnInitialize();

        public int Id => {expectedModelPropertyName}.Id;

        public bool IsDeveloper
        {{
            get => {expectedModelPropertyName}.IsDeveloper;
            set
            {{
                if ({expectedModelPropertyName}.IsDeveloper != value)
                {{
                    {expectedModelPropertyName}.IsDeveloper = value;
                    OnPropertyChanged(""IsDeveloper"");
                }}
            }}
        }}

        protected MyCode.Employee { expectedModelPropertyName } {{ get; set; }}
    }}
}}
");
        }

    }
}
