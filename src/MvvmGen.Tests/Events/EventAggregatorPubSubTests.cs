// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
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

            var eventSubscriber = new CustomerAddedEventSubscriber();
            eventAggregator.RegisterSubscriber(eventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));

            Assert.NotNull(eventSubscriber.ReceivedEvent);
            Assert.Equal(5, eventSubscriber.ReceivedEvent?.CustomerId);
        }

        [Fact]
        public void ShouldPushEventsToRightSubscribers()
        {
            var eventAggregator = new EventAggregator();

            var customerAddedEventSubscriber = new CustomerAddedEventSubscriber();
            eventAggregator.RegisterSubscriber(customerAddedEventSubscriber);

            var customerDeletedEventSubscriber = new CustomerDeletedEventSubscriber();
            eventAggregator.RegisterSubscriber(customerDeletedEventSubscriber);

            eventAggregator.Publish(new CustomerAddedEvent(5));
            eventAggregator.Publish(new CustomerDeletedEvent(7));

            Assert.Equal(5, customerAddedEventSubscriber.ReceivedEvent?.CustomerId);
            Assert.Equal(7, customerDeletedEventSubscriber.ReceivedEvent?.CustomerId);
        }

        [Fact]
        public void ShouldThrowIfSubscriberIsNull()
        {
            var eventAggregator = new EventAggregator();
            var ex = Assert.Throws<ArgumentNullException>(() => eventAggregator.RegisterSubscriber<object>(null!));

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

            var customerAllEventsSeparateInterfaceSubscriber = new CustomerAllEventsSubscriberSeparateInterfaces();
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
