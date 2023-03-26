// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using MvvmGen.SourceGenerators.Extensions;
using Xunit;

namespace MvvmGen.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ShouldBeEqual1()
        {
            List<string> list1 = new() { "FirstName" };
            List<string> list2 = new() { "FirstName" };

            Assert.True(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            List<string> list1 = new() { "FirstName" };
            string[] list2 = new[] { "FirstName" };

            Assert.True(list1.SequenceEqualWithNullCheck(list2));
        }


        [Fact]
        public void ShouldBeEqual3()
        {
            List<string>? list1 = null;
            List<string> list2 = new();

            Assert.True(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldBeEqual4()
        {
            List<string> list1 = new();
            List<string>? list2 = null;

            Assert.True(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldBeEqual5()
        {
            List<string>? list1 = null;
            List<string>? list2 = null;

            Assert.True(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            List<string> list1 = new() { "FirstName" };
            List<string> list2 = new() { "FirstName", "Lastname" };

            Assert.False(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            List<string> list1 = new() { "FirstName" };
            List<string> list2 = new() { "Lastname" };

            Assert.False(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            List<string> list1 = new() { "FirstName" };
            List<string> list2 = new() { };

            Assert.False(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldNotBeEqual4()
        {
            List<string> list1 = new() { "FirstName" };
            List<string>? list2 = null;

            Assert.False(list1.SequenceEqualWithNullCheck(list2));
        }

        [Fact]
        public void ShouldNotBeEqual5()
        {
            List<string>? list1 = null;
            List<string>? list2 = new() { "FirstName" };

            Assert.False(list1.SequenceEqualWithNullCheck(list2));
        }
    }
}
