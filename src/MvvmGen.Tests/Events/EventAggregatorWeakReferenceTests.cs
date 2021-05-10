// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

using System;
using Xunit;

namespace MvvmGen.Events
{
    public class EventAggregatorWeakReferenceTests
    {
        [InlineData(false, true)]
        [InlineData(true, false)]
        [Theory]
        public void ShouldNotHoldStrongReferenceToSubscribers(bool expectedIsAliveValue, bool garbageCollect)
        {
            var eventAggregator = new EventAggregator();

            CreateAndRegisterSubscriber(eventAggregator);

            if (garbageCollect)
            {
                GC.Collect();
            }

            Assert.Equal(expectedIsAliveValue, _weakReference?.IsAlive);
        }

        [InlineData(0, true)]
        [InlineData(1, false)]
        [Theory]
        public void ShouldRemoveDeadSubscribersAfterPublish(int expectedNumberOfSubscribers, bool garbageCollect)
        {
            var eventAggregator = new EventAggregator();

            CreateAndRegisterSubscriber(eventAggregator);

            if (garbageCollect)
            {
                GC.Collect();
            }

            eventAggregator.Publish(new CustomerAddedEvent(9));

            Assert.Equal(expectedNumberOfSubscribers, eventAggregator._subscribersByEvent[typeof(CustomerAddedEvent)].Count);
        }

        [InlineData(0, true)]
        [InlineData(1, false)]
        [Theory]
        public void ShouldRemoveDeadSubscribersAfterUnregisterASubscriber(int expectedNumberOfSubscribers, bool garbageCollect)
        {
            var eventAggregator = new EventAggregator();

            CreateAndRegisterSubscriber(eventAggregator);
            var anotherSubscriber = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(anotherSubscriber);

            if (garbageCollect)
            {
                GC.Collect();
            }

            eventAggregator.UnregisterSubscriber(anotherSubscriber);

            Assert.Equal(expectedNumberOfSubscribers, eventAggregator._subscribersByEvent[typeof(CustomerAddedEvent)].Count);
        }

        private WeakReference? _weakReference;

        private void CreateAndRegisterSubscriber(IEventAggregator eventAggregator)
        {
            var sub = new TestEventSubscriber<CustomerAddedEvent>();
            eventAggregator.RegisterSubscriber(sub);

            _weakReference = new WeakReference(sub);
        }
    }
}
