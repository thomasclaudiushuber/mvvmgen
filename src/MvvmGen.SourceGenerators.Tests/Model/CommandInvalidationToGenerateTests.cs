// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using Xunit;

namespace MvvmGen.Model
{
    public class CommandInvalidationToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var item1 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });
            var item2 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });

            Assert.Equal(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var item1 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });
            var item2 = new CommandInvalidationToGenerate("LastName", new[] { "SaveCommand" });

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var item1 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });
            var item2 = new CommandInvalidationToGenerate("FirstName", new[] { "DeleteCommand" });

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var item1 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });
            var item2 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand", "DeleteCommand" });

            Assert.NotEqual(item1, item2);
        }

        [Fact]
        public void ShouldNotBeEqual4()
        {
            var item1 = new CommandInvalidationToGenerate("FirstName", null!); // Null is not passed by the generator, but let's test it anyway
            var item2 = new CommandInvalidationToGenerate("FirstName", new[] { "SaveCommand" });

            Assert.NotEqual(item1, item2);
        }
    }
}
