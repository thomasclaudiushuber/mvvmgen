// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.Model
{
    public class CommandToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var item1 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };
            var item2 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var item1 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };
            var item2 = new CommandToGenerate(new CommandMethod("SaveChanged"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var item1 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };
            var item2 = new CommandToGenerate(new CommandMethod("Save"), "LastName") { CanExecuteMethod = new CommandMethod("CanSave") };

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var item1 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSave") };
            var item2 = new CommandToGenerate(new CommandMethod("Save"), "FirstName") { CanExecuteMethod = new CommandMethod("CanSaveChanged") };

            Assert.NotEqual(item1, item2);
        }
    }

    public class CommandMethodTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var method1 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = false };
            var method2 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = false };

            Assert.Equal(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var method1 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = false };
            var method2 = new CommandMethod("Delete") { IsAwaitable = false, HasParameter = false };

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var method1 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = false };
            var method2 = new CommandMethod("Save") { IsAwaitable = true, HasParameter = false };

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var method1 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = false };
            var method2 = new CommandMethod("Save") { IsAwaitable = false, HasParameter = true };

            Assert.NotEqual(method1, method2);
        }
    }
}
