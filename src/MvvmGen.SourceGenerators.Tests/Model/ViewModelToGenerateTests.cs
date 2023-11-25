// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using Xunit;

namespace MvvmGen.Model
{
    public class ViewModelToGenerateTests
    {
        private ViewModelToGenerate _viewModelToGenerate1;
        private ViewModelToGenerate _viewModelToGenerate2;

        public ViewModelToGenerateTests()
        {
            _viewModelToGenerate1 = new ViewModelToGenerate("MyClass", "MyNamespace");
            _viewModelToGenerate2 = new ViewModelToGenerate("MyClass", "MyNamespace");
            FillAllProperties(_viewModelToGenerate1);
            FillAllProperties(_viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual()
        {
            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentNamespace()
        {
            var viewModelToGenerate2 = new ViewModelToGenerate("MyClass", "DifferentNamespace");
            FillAllProperties(viewModelToGenerate2);

            Assert.NotEqual(_viewModelToGenerate1, viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentClass()
        {
            var viewModelToGenerate2 = new ViewModelToGenerate("DifferentClass", "MyNamespace");
            FillAllProperties(viewModelToGenerate2);

            Assert.NotEqual(_viewModelToGenerate1, viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentClassAccessModifier1()
        {
            _viewModelToGenerate2.ClassAccessModifier = "private";

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentClassAccessModifier2()
        {
            _viewModelToGenerate2.ClassAccessModifier = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentGenerateConstructor()
        {
            _viewModelToGenerate2.GenerateConstructor = !_viewModelToGenerate1.GenerateConstructor;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentInheritFromViewModelBase()
        {
            _viewModelToGenerate2.InheritFromViewModelBase = !_viewModelToGenerate1.InheritFromViewModelBase;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentIsEventSubscriber()
        {
            _viewModelToGenerate2.IsEventSubscriber = !_viewModelToGenerate1.IsEventSubscriber;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentWrappedModelType1()
        {
            _viewModelToGenerate2.WrappedModelType = "Person";

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentWrappedModelType2()
        {
            _viewModelToGenerate2.WrappedModelType = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentWrappedModelPropertyName1()
        {
            _viewModelToGenerate2.WrappedModelPropertyName = "CustomModelProperty";

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentWrappedModelPropertyName2()
        {
            _viewModelToGenerate2.WrappedModelPropertyName = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandFactoryType1()
        {
            _viewModelToGenerate2.CommandFactoryType = "typeof(MyNewCommandFactory)";

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandFactoryType2()
        {
            _viewModelToGenerate2.CommandFactoryType = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentPropertiesToGenerate1()
        {
            _viewModelToGenerate2.PropertiesToGenerate = new List<PropertyToGenerate>();

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentPropertiesToGenerate2()
        {
            var list = (List<PropertyToGenerate>)_viewModelToGenerate2.PropertiesToGenerate!;

            list.Add(new PropertyToGenerate("LastName", "string", "_lastName"));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentPropertiesToGenerate3()
        {
            var list = (List<PropertyToGenerate>)_viewModelToGenerate2.PropertiesToGenerate!;

            var originalPropertyToGenerate = list[0];

            list.Clear();
            list.Add(new PropertyToGenerate("FirstNameChanged",
                originalPropertyToGenerate.PropertyType,
                originalPropertyToGenerate.BackingField,
                originalPropertyToGenerate.IsReadOnly));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            _viewModelToGenerate1.PropertiesToGenerate = null;
            _viewModelToGenerate2.PropertiesToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandsToGenerate1()
        {
            _viewModelToGenerate2.CommandsToGenerate = new List<CommandToGenerate>();

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandsToGenerate2()
        {
            var list = (List<CommandToGenerate>)_viewModelToGenerate2.CommandsToGenerate!;

            list.Add(new CommandToGenerate(new CommandMethod("OnDelete"), "DeleteCommand"));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandsToGenerate3()
        {
            var list = (List<CommandToGenerate>)_viewModelToGenerate2.CommandsToGenerate!;

            var originalCommandToGenerate = list[0];

            list.Clear();
            list.Add(new CommandToGenerate(originalCommandToGenerate.ExecuteMethod, "RemoveCommand"));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual3()
        {
            _viewModelToGenerate1.CommandsToGenerate = null;
            _viewModelToGenerate2.CommandsToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandInvalidationsToGenerate1()
        {
            _viewModelToGenerate2.CommandInvalidationsToGenerate = new List<CommandInvalidationToGenerate>();

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandInvalidationsToGenerate2()
        {
            var list = (List<CommandInvalidationToGenerate>)_viewModelToGenerate2.CommandInvalidationsToGenerate!;

            list.Add(new CommandInvalidationToGenerate("LastName", new[] { "SaveCommand" }));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentCommandInvalidationsToGenerate3()
        {
            var list = (List<CommandInvalidationToGenerate>)_viewModelToGenerate2.CommandInvalidationsToGenerate!;

            var originalCommandInvalidationToGenerate = list[0];

            list.Clear();
            list.Add(new CommandInvalidationToGenerate("LastName", new[] { "SaveCommand" }));

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual4()
        {
            _viewModelToGenerate1.CommandInvalidationsToGenerate = null;
            _viewModelToGenerate2.CommandInvalidationsToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentInjectionsToGenerate1()
        {
            _viewModelToGenerate2.InjectionsToGenerate = new List<InjectionToGenerate>();

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentInjectionsToGenerate2()
        {
            var list = (List<InjectionToGenerate>)_viewModelToGenerate2.InjectionsToGenerate!;

            list.Add(new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "private" });

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentInjectionsToGenerate3()
        {
            var list = (List<InjectionToGenerate>)_viewModelToGenerate2.InjectionsToGenerate!;

            var originalInjectionToGenerate = list[0];

            list.Clear();
            list.Add(new InjectionToGenerate("IChangedDataProvider", originalInjectionToGenerate.PropertyName)
            {
                PropertyAccessModifier = originalInjectionToGenerate.PropertyAccessModifier
            });

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual5()
        {
            _viewModelToGenerate1.InjectionsToGenerate = null;
            _viewModelToGenerate2.InjectionsToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentViewModelFactoryToGenerate1()
        {
            _viewModelToGenerate2.ViewModelFactoryToGenerate = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentViewModelFactoryToGenerate2()
        {
            _viewModelToGenerate2.ViewModelFactoryToGenerate = new FactoryToGenerate("DifferentFactoryName", "SuperInterface", "IViewModel");

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual6()
        {
            _viewModelToGenerate1.ViewModelFactoryToGenerate = null;
            _viewModelToGenerate2.ViewModelFactoryToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentViewModelInterfaceToGenerate1()
        {
            _viewModelToGenerate2.ViewModelInterfaceToGenerate = null;

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentViewModelInterfaceToGenerate2()
        {
            _viewModelToGenerate2.ViewModelInterfaceToGenerate = new InterfaceToGenerate("DifferentInterfaceName");

            Assert.NotEqual(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        [Fact]
        public void ShouldBeEqual7()
        {
            _viewModelToGenerate1.ViewModelInterfaceToGenerate = null;
            _viewModelToGenerate2.ViewModelInterfaceToGenerate = null;

            Assert.Equal(_viewModelToGenerate1, _viewModelToGenerate2);
        }

        private static void FillAllProperties(ViewModelToGenerate viewModelToGenerate)
        {
            viewModelToGenerate.ClassAccessModifier = "public";
            viewModelToGenerate.GenerateConstructor = true;
            viewModelToGenerate.InheritFromViewModelBase = true;
            viewModelToGenerate.IsEventSubscriber = true;
            viewModelToGenerate.WrappedModelType = "Employee";
            viewModelToGenerate.WrappedModelPropertyName = "EmployeeModel";
            viewModelToGenerate.CommandFactoryType = "typeof(MyCommandFactory)";

            viewModelToGenerate.PropertiesToGenerate = new List<PropertyToGenerate>
            {
                new PropertyToGenerate("FirstName","string","_firstName",false)
            };

            viewModelToGenerate.CommandsToGenerate = new List<CommandToGenerate>
            {
                new CommandToGenerate(new CommandMethod("OnSave"),"SaveCommand")
            };

            viewModelToGenerate.CommandInvalidationsToGenerate = new List<CommandInvalidationToGenerate>
            {
                new CommandInvalidationToGenerate("FirstName",new []{"SaveCommand"})
            };

            viewModelToGenerate.InjectionsToGenerate = new List<InjectionToGenerate> {
                new InjectionToGenerate("IEventAggregator","EventAggregator"){  PropertyAccessModifier ="public"}
            };

            viewModelToGenerate.ViewModelFactoryToGenerate = new FactoryToGenerate("FactoryClassName", "FactoryInterfaceName", "CustomReturnType");
            viewModelToGenerate.ViewModelInterfaceToGenerate = new InterfaceToGenerate("InterfaceName")
            {
                Properties = new List<InterfaceProperty> { new InterfaceProperty("FirstName", "string", false) },
                Methods = new List<InterfaceMethod>
                {
                    new InterfaceMethod("OnSave","void")
                    {
                        Parameters = new List<InterfaceMethodParameter>{new InterfaceMethodParameter("parameter","object")}
                    }
                }
            };
        }
    }
}
