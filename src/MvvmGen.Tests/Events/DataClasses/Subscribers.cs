// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.Events
{
    public class CustomerAddedEventSubscriber : IEventSubscriber<CustomerAddedEvent>
    {
        public CustomerAddedEvent? ReceivedEvent { get; private set; }

        public void OnEvent(CustomerAddedEvent theEvent)
        {
            ReceivedEvent = theEvent;
        }
    }

    public class CustomerDeletedEventSubscriber : IEventSubscriber<CustomerDeletedEvent>
    {
        public CustomerDeletedEvent? ReceivedEvent { get; private set; }

        public void OnEvent(CustomerDeletedEvent theEvent)
        {
            ReceivedEvent = theEvent;
        }
    }

    public class CustomerAllEventsSubscriberSeparateInterfaces : IEventSubscriber<CustomerDeletedEvent>, IEventSubscriber<CustomerAddedEvent>
    {
        public CustomerDeletedEvent? ReceivedCustomerDeletedEvent { get; private set; }
        public CustomerAddedEvent? ReceivedCustomerAddedEvent { get; private set; }

        public void OnEvent(CustomerDeletedEvent theEvent)
        {
            ReceivedCustomerDeletedEvent = theEvent;
        }

        public void OnEvent(CustomerAddedEvent theEvent)
        {
            ReceivedCustomerAddedEvent = theEvent;
        }
    }

    public class CustomerAllEventsSubscriberSingleInterface : IEventSubscriber<CustomerDeletedEvent, CustomerAddedEvent>
    {
        public CustomerDeletedEvent? ReceivedCustomerDeletedEvent { get; private set; }
        public CustomerAddedEvent? ReceivedCustomerAddedEvent { get; private set; }

        public void OnEvent(CustomerDeletedEvent theEvent)
        {
            ReceivedCustomerDeletedEvent = theEvent;
        }

        public void OnEvent(CustomerAddedEvent theEvent)
        {
            ReceivedCustomerAddedEvent = theEvent;
        }
    }
}
