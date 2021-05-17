// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See LICENSE file in repository root
// ***********************************************************************

using System;
using Xunit;

namespace MvvmGen.Events
{
    public class EventAggregatorPubSubTests
    {
        [Fact]
        public void ShouldSubscribeToEvent()
        {
            var eventAggregator = new EventAggregator();

            var eventSubscriber = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(eventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));

            Assert.Single(eventSubscriber.ReceivedEvents);
            Assert.Equal(5, eventSubscriber.ReceivedEvents[0].CustomerId);
        }

        [Fact]
        public void ShouldReceiveEventThreeTimes()
        {
            var eventAggregator = new EventAggregator();

            var eventSubscriber = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(eventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.Publish(new CustomerAddedEvent(6));
            eventAggregator.Publish(new CustomerAddedEvent(7));

            Assert.Equal(3, eventSubscriber.ReceivedEvents.Count);
            Assert.Equal(5, eventSubscriber.ReceivedEvents[0].CustomerId);
            Assert.Equal(6, eventSubscriber.ReceivedEvents[1].CustomerId);
            Assert.Equal(7, eventSubscriber.ReceivedEvents[2].CustomerId);
        }

        [Fact]
        public void ShouldUnsubscribeFromEvent()
        {
            var eventAggregator = new EventAggregator();

            var eventSubscriber = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(eventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.UnregisterSubscriber(eventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(6));

            Assert.Single(eventSubscriber.ReceivedEvents);
            Assert.Equal(5, eventSubscriber.ReceivedEvents[0].CustomerId);
        }

        [Fact]
        public void ShouldPushEventsToRightSubscribers()
        {
            var eventAggregator = new EventAggregator();

            var customerAddedEventSubscriber = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(customerAddedEventSubscriber);

            var customerDeletedEventSubscriber = new TestEventSubscriber<CustomerDeletedEvent>();
            eventAggregator.RegisterSubscriber(customerDeletedEventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.Publish(new CustomerDeletedEvent(7));

            Assert.Single(customerAddedEventSubscriber.ReceivedEvents);
            Assert.Equal(5, customerAddedEventSubscriber.ReceivedEvents[0].CustomerId);
            Assert.Single(customerDeletedEventSubscriber.ReceivedEvents);
            Assert.Equal(7, customerDeletedEventSubscriber.ReceivedEvents[0].CustomerId);
        }

        [Fact]
        public void ShouldThrowIfSubscriberIsNullWhenRegistering()
        {
            var eventAggregator = new EventAggregator();
            var ex = Assert.Throws<ArgumentNullException>(() => eventAggregator.RegisterSubscriber<object>(null!));

            Assert.Equal("subscriber", ex.ParamName);
        }

        [Fact]
        public void ShouldThrowIfSubscriberIsNullWhenUnregistering()
        {
            var eventAggregator = new EventAggregator();
            var ex = Assert.Throws<ArgumentNullException>(() => eventAggregator.UnregisterSubscriber<object>(null!));

            Assert.Equal("subscriber", ex.ParamName);
        }

        [Fact]
        public void ShouldNotFailWithoutSubscribers()
        {
            var eventAggregator = new EventAggregator();

            eventAggregator.Publish(new CustomerAddedEvent(5));
        }

        [Fact]
        public void ShouldWorkWithMultipleEventSubscriptionsUsingSeparateInterfaces()
        {
            var eventAggregator = new EventAggregator();

            var customerAllEventsSeparateInterfaceSubscriber = new TestAllEventsSubscriberSeparateInterfaces();
            eventAggregator.RegisterSubscriber(customerAllEventsSeparateInterfaceSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.Publish(new CustomerDeletedEvent(7));

            Assert.Equal(5, customerAllEventsSeparateInterfaceSubscriber.ReceivedCustomerAddedEvent?.CustomerId);
            Assert.Equal(7, customerAllEventsSeparateInterfaceSubscriber.ReceivedCustomerDeletedEvent?.CustomerId);
        }

        [Fact]
        public void ShouldWorkWithMultipleEventSubscriptionsUsingSingleInterface()
        {
            var eventAggregator = new EventAggregator();

            var customerAllEventsSingleInterfaceSubscriber = new CustomerAllEventsSubscriberSingleInterface();
            eventAggregator.RegisterSubscriber(customerAllEventsSingleInterfaceSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.Publish(new CustomerDeletedEvent(7));

            Assert.Equal(5, customerAllEventsSingleInterfaceSubscriber.ReceivedCustomerAddedEvent?.CustomerId);
            Assert.Equal(7, customerAllEventsSingleInterfaceSubscriber.ReceivedCustomerDeletedEvent?.CustomerId);
        }
    }
}
