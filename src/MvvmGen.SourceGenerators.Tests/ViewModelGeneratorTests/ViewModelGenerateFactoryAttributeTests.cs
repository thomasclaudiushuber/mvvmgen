﻿// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.SourceGenerators
{
    public class ViewModelGenerateFactoryAttributeTests : ViewModelGeneratorTestsBase
    {
        [InlineData("public", "public")]
        [InlineData("internal", "internal")]
        [InlineData("internal", "")]
        [Theory]
        public void GenerateFactoryClassForViewModel(string expectedAccessModifierFactory, string accessModifierViewModel)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  public interface IEmployeeDataProvider {{}}

  [Inject(typeof(IEmployeeDataProvider)))]
  [ViewModelGenerateFactory]
  [ViewModel]
  {accessModifierViewModel} partial class EmployeeViewModel
  {{
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MyCode.IEmployeeDataProvider employeeDataProvider)
        {{
            this.EmployeeDataProvider = employeeDataProvider;
            this.OnInitialize();
        }}

        partial void OnInitialize();

        protected MyCode.IEmployeeDataProvider EmployeeDataProvider {{ get; private set; }}
    }}

    {expectedAccessModifierFactory} interface IEmployeeViewModelFactory : IViewModelFactory<EmployeeViewModel> {{ }}

    {expectedAccessModifierFactory} class EmployeeViewModelFactory : IEmployeeViewModelFactory
    {{
        public EmployeeViewModelFactory(MyCode.IEmployeeDataProvider employeeDataProvider)
        {{
            this.EmployeeDataProvider = employeeDataProvider;
        }}

        protected MyCode.IEmployeeDataProvider EmployeeDataProvider {{ get; private set; }}

        public EmployeeViewModel Create() => new EmployeeViewModel(EmployeeDataProvider);
    }}
}}
");
        }

        [InlineData("EmpVmFactory", "IEmpVmFactory", "InterfaceName=\"IEmpVmFactory\",ClassName=\"EmpVmFactory\"")]
        [InlineData("EmpVmFactory", "IEmpVmFactory", "ClassName=\"EmpVmFactory\",InterfaceName=\"IEmpVmFactory\"")]
        [InlineData("EmployeeViewModelFactory", "IEmpVmFactory", "InterfaceName=\"IEmpVmFactory\"")]
        [InlineData("EmpVmFactory", "IEmployeeViewModelFactory", "ClassName=\"EmpVmFactory\"")]
        [InlineData("EmployeeViewModelFactory", "IEmployeeViewModelFactory", "")]
        [Theory]
        public void GenerateFactoryClassForViewModelWithSpecifiedNames(string expectedClassName, string expectedInterfaceName, string attributeArguments)
        {
            ShouldGenerateExpectedCode(
      $@"using MvvmGen;

namespace MyCode
{{
  public interface IEmployeeDataProvider {{}}

  [Inject(typeof(IEmployeeDataProvider)))]
  [ViewModelGenerateFactory({attributeArguments})]
  [ViewModel]
  partial class EmployeeViewModel
  {{
  }}
}}",
      $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MyCode.IEmployeeDataProvider employeeDataProvider)
        {{
            this.EmployeeDataProvider = employeeDataProvider;
            this.OnInitialize();
        }}

        partial void OnInitialize();

        protected MyCode.IEmployeeDataProvider EmployeeDataProvider {{ get; private set; }}
    }}

    internal interface {expectedInterfaceName} : IViewModelFactory<EmployeeViewModel> {{ }}

    internal class {expectedClassName} : {expectedInterfaceName}
    {{
        public {expectedClassName}(MyCode.IEmployeeDataProvider employeeDataProvider)
        {{
            this.EmployeeDataProvider = employeeDataProvider;
        }}

        protected MyCode.IEmployeeDataProvider EmployeeDataProvider {{ get; private set; }}

        public EmployeeViewModel Create() => new EmployeeViewModel(EmployeeDataProvider);
    }}
}}
");
        }

        [Fact]
        public void GenerateFactoryClassWithEventAggregatorConstructorParameter()
        {
            ShouldGenerateExpectedCode(
                $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
    [ViewModelGenerateFactory]
    [ViewModel]
    public partial class EmployeeViewModel : IEventSubscriber<string>
    {{
        public void OnEvent(string eventData)
        {{
        }}
    }}
}}",
                $@"{AutoGeneratedTopContent}

namespace MyCode
{{
    partial class EmployeeViewModel : global::MvvmGen.ViewModels.ViewModelBase
    {{
        public EmployeeViewModel(MvvmGen.Events.IEventAggregator eventAggregator)
        {{
            eventAggregator.RegisterSubscriber(this);
            this.OnInitialize();
        }}

        partial void OnInitialize();
    }}

    public interface IEmployeeViewModelFactory : IViewModelFactory<EmployeeViewModel> {{ }}

    public class EmployeeViewModelFactory : IEmployeeViewModelFactory
    {{
        public EmployeeViewModelFactory(MvvmGen.Events.IEventAggregator eventAggregator)
        {{
            this.EventAggregator = eventAggregator;
        }}

        protected MvvmGen.Events.IEventAggregator EventAggregator {{ get; private set; }}

        public EmployeeViewModel Create() => new EmployeeViewModel(EventAggregator);
    }}
}}
");
        }

        [Fact]
        public void GenerateFactoryClassWithViewModelInterfaceAsReturnType()
        {
            ShouldGenerateExpectedCode(
                $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
    [ViewModelGenerateInterface]
    [ViewModelGenerateFactory]
    [ViewModel]
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
    }}

    public interface IEmployeeViewModel
    {{
    }}

    public interface IEmployeeViewModelFactory : IViewModelFactory<IEmployeeViewModel> {{ }}

    public class EmployeeViewModelFactory : IEmployeeViewModelFactory
    {{
        public EmployeeViewModelFactory()
        {{
        }}

        public IEmployeeViewModel Create() => new EmployeeViewModel();
    }}
}}
");
        }

        [Fact]
        public void GenerateFactoryClassWithCustomReturnTypeWithViewModelGenerateInterfaceAttributeSet()
        {
            ShouldGenerateExpectedCode(
                $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
    public interface CustomInterface {{ }}

    [ViewModelGenerateInterface]
    [ViewModelGenerateFactory(ReturnType = typeof(CustomInterface))]
    [ViewModel]
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
    }}

    public interface IEmployeeViewModel
    {{
    }}

    public interface IEmployeeViewModelFactory : IViewModelFactory<MyCode.CustomInterface> {{ }}

    public class EmployeeViewModelFactory : IEmployeeViewModelFactory
    {{
        public EmployeeViewModelFactory()
        {{
        }}

        public MyCode.CustomInterface Create() => new EmployeeViewModel();
    }}
}}
");
        }

        [Fact]
        public void GenerateFactoryClassWithCustomReturnTypeWithViewModelGenerateInterfaceAttributeNotSet()
        {
            ShouldGenerateExpectedCode(
                $@"using MvvmGen;
using MvvmGen.Events;

namespace MyCode
{{
    public interface CustomInterface {{ }}

    [ViewModelGenerateFactory(ReturnType = typeof(CustomInterface))]
    [ViewModel]
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
    }}

    public interface IEmployeeViewModelFactory : IViewModelFactory<MyCode.CustomInterface> {{ }}

    public class EmployeeViewModelFactory : IEmployeeViewModelFactory
    {{
        public EmployeeViewModelFactory()
        {{
        }}

        public MyCode.CustomInterface Create() => new EmployeeViewModel();
    }}
}}
");
        }
    }
}
