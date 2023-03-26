// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.Model
{
    public class FactoryToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var item1 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");
            var item2 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var item1 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");
            var item2 = new FactoryToGenerate("ChangedFactory", "IMyFactory", "IMyViewModel");

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var item1 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");
            var item2 = new FactoryToGenerate("MyFactory", "ChangedInterface", "IMyViewModel");

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var item1 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");
            var item2 = new FactoryToGenerate("MyFactory", "IMyFactory", "ChangedReturnType");

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual4()
        {
            var item1 = new FactoryToGenerate("MyFactory", "IMyFactory", null);
            var item2 = new FactoryToGenerate("MyFactory", "IMyFactory", "IMyViewModel");

            Assert.NotEqual(item1, item2);
        }
    }
}
