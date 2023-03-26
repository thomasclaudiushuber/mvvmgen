// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using Xunit;

namespace MvvmGen.Model
{
    public class InterfaceToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var item1 = new InterfaceToGenerate("IEmployeeViewModel");
            var item2 = new InterfaceToGenerate("IEmployeeViewModel");

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            var item1 = new InterfaceToGenerate("IEmployeeViewModel");
            var item2 = new InterfaceToGenerate("IEmployeeViewModel");

            static void SetPropertiesAndMethods(InterfaceToGenerate interfaceToGenerate)
            {
                interfaceToGenerate.Properties = new List<InterfaceProperty> { new InterfaceProperty("FirstName", "string", false) };
                interfaceToGenerate.Methods = new List<InterfaceMethod> { new InterfaceMethod("Save", "void") };
            }

            SetPropertiesAndMethods(item1);
            SetPropertiesAndMethods(item2);

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentNames()
        {
            var item1 = new InterfaceToGenerate("IEmployeeViewModel");
            var item2 = new InterfaceToGenerate("IPersonViewModel");

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentProperties()
        {
            var item1 = new InterfaceToGenerate("IEmployeeViewModel")
            {
                Properties = new List<InterfaceProperty> { new InterfaceProperty("FirstName", "string", false) }
            };

            var item2 = new InterfaceToGenerate("IEmployeeViewModel");

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentMethods()
        {
            var item1 = new InterfaceToGenerate("IEmployeeViewModel")
            {
                Methods = new List<InterfaceMethod> { new InterfaceMethod("Save", "string") }
            };

            var item2 = new InterfaceToGenerate("IEmployeeViewModel");

            Assert.NotEqual(item1, item2);
        }
    }

    public class InterfacePropertyTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var property1 = new InterfaceProperty("FirstName", "string", false);
            var property2 = new InterfaceProperty("FirstName", "string", false);

            Assert.Equal(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentPropertyName()
        {
            var property1 = new InterfaceProperty("FirstName", "string", false);
            var property2 = new InterfaceProperty("LastName", "string", false);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentPropertyType()
        {
            var property1 = new InterfaceProperty("FirstName", "string", false);
            var property2 = new InterfaceProperty("FirstName", "int", false);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentReadonlyValue()
        {
            var property1 = new InterfaceProperty("FirstName", "string", false);
            var property2 = new InterfaceProperty("FirstName", "string", true);

            Assert.NotEqual(property1, property2);
        }
    }

    public class InterfaceMethodTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var method1 = new InterfaceMethod("Save", "void");
            var method2 = new InterfaceMethod("Save", "void");

            Assert.Equal(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentName()
        {
            var method1 = new InterfaceMethod("Save", "void");
            var method2 = new InterfaceMethod("Delete", "void");

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentReturnType()
        {
            var method1 = new InterfaceMethod("Save", "void");
            var method2 = new InterfaceMethod("Save", "int");

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            var method1 = new InterfaceMethod("Save", "void")
            {
                Parameters = new List<InterfaceMethodParameter> { new InterfaceMethodParameter("firstname", "string") }
            };

            var method2 = new InterfaceMethod("Save", "void")
            {
                Parameters = new[] { new InterfaceMethodParameter("firstname", "string") }
            };

            Assert.Equal(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentParameterNames()
        {
            var method1 = new InterfaceMethod("Save", "void")
            {
                Parameters = new List<InterfaceMethodParameter> { new InterfaceMethodParameter("firstname", "string") }
            };

            var method2 = new InterfaceMethod("Save", "void")
            {
                Parameters = new[] { new InterfaceMethodParameter("lastname", "string") }
            };

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentParameterNumber()
        {
            var method1 = new InterfaceMethod("Save", "void")
            {
                Parameters = new List<InterfaceMethodParameter> { new InterfaceMethodParameter("firstname", "string") }
            };

            var method2 = new InterfaceMethod("Save", "void")
            {
                Parameters = new[]
                {
                    new InterfaceMethodParameter("firstname", "string"),
                    new InterfaceMethodParameter("lastname","string")
                }
            };

            Assert.NotEqual(method1, method2);
        }
    }

    public class InterfaceMethodParameterTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var property1 = new InterfaceMethodParameter("firstname", "string");
            var property2 = new InterfaceMethodParameter("firstname", "string");

            Assert.Equal(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentParameterName()
        {
            var property1 = new InterfaceMethodParameter("firstname", "string");
            var property2 = new InterfaceMethodParameter("lastname", "string");

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqualDifferentParameterType()
        {
            var property1 = new InterfaceMethodParameter("firstname", "string");
            var property2 = new InterfaceMethodParameter("firstname", "int");

            Assert.NotEqual(property1, property2);
        }
    }
}
