// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.Model
{
    public class InjectionToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var item1 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "public" };
            var item2 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "public" };

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var item1 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "public" };
            var item2 = new InjectionToGenerate("IChangedProvider", "DataProvider") { PropertyAccessModifier = "public" };

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var item1 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "public" };
            var item2 = new InjectionToGenerate("IDataProvider", "ChangedProvider") { PropertyAccessModifier = "public" };

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var item1 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "public" };
            var item2 = new InjectionToGenerate("IDataProvider", "DataProvider") { PropertyAccessModifier = "private" };

            Assert.NotEqual(item1, item2);
        }
    }
}
