// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System.Collections.Generic;
using Xunit;

namespace MvvmGen.Model
{
    public class PropertyToGenerateTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_firstName", false);

            Assert.Equal(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("LastName", "string", "_firstName", false);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "int", "_firstName", false);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_lastName", false);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual4()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_firstName", true);

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldBeEqual2()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_firstName", false);

            FillAllProperties(property1);
            FillAllProperties(property2);

            Assert.Equal(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual5()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_firstName", false);

            FillAllProperties(property1);
            FillAllProperties(property2);

            ((List<string>)property2.PropertiesToInvalidate!)[0] = "ChangedProperty";

            Assert.NotEqual(property1, property2);
        }

        [Fact]
        public void ShouldNotBeEqual6()
        {
            var property1 = new PropertyToGenerate("FirstName", "string", "_firstName", false);
            var property2 = new PropertyToGenerate("FirstName", "string", "_firstName", false);

            FillAllProperties(property1);
            FillAllProperties(property2);

            ((List<PropertyEventPublication>)property2.EventsToPublish!)[0] = new PropertyEventPublication("ChangedEvent");

            Assert.NotEqual(property1, property2);
        }

        private static void FillAllProperties(PropertyToGenerate propertyToGenerate)
        {
            propertyToGenerate.PropertiesToInvalidate = new List<string> { "FullName" };
            propertyToGenerate.EventsToPublish = new List<PropertyEventPublication> { new PropertyEventPublication("SavedEvent") };
        }
    }

    public class PropertyEventPublicationTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var event1 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };
            var event2 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };

            Assert.Equal(event1, event2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var event1 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };
            var event2 = new PropertyEventPublication("MyEventChanged") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };

            Assert.NotEqual(event1, event2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var event1 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };
            var event2 = new PropertyEventPublication("MyEvent") { PublishCondition = "_lastName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };

            Assert.NotEqual(event1, event2);
        }

        [Fact]
        public void ShouldNotBeEqual3()
        {
            var event1 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };
            var event2 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "ChangedAggregator", EventConstructorArgs = "item1" };

            Assert.NotEqual(event1, event2);
        }

        [Fact]
        public void ShouldNotBeEqual4()
        {
            var event1 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "item1" };
            var event2 = new PropertyEventPublication("MyEvent") { PublishCondition = "_firstName is not null", EventAggregatorMemberName = "MyAggregator", EventConstructorArgs = "itemChanged" };

            Assert.NotEqual(event1, event2);
        }
    }

    public class PropertyMethodCallTests
    {
        [Fact]
        public void ShouldBeEqual()
        {
            var method1 = new PropertyMethodCall("Save");
            var method2 = new PropertyMethodCall("Save");

            Assert.Equal(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqual1()
        {
            var method1 = new PropertyMethodCall("Save");
            var method2 = new PropertyMethodCall("Delete");

            Assert.NotEqual(method1, method2);
        }

        [Fact]
        public void ShouldNotBeEqual2()
        {
            var method1 = new PropertyMethodCall("Save") { MethodArgs = "item1" };
            var method2 = new PropertyMethodCall("Save");

            Assert.NotEqual(method1, method2);
        }
    }
}
