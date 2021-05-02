using System;
using Xunit;

namespace MvvmGen.Events
{
  public class EventAggregatorTests
  {
    [Fact]
    public void ShouldSubscribeToEvent()
    {
      var eventAggregator = new EventAggregator();

      int? receivedValue = null;
      EventHandler<int> myHandler = (s, e) => receivedValue = e;
      eventAggregator.Subscribe<int>(myHandler);
      eventAggregator.Publish<int>(5);

      Assert.NotNull(receivedValue);
      Assert.Equal(5, receivedValue.Value);
    }

    [Fact]
    public void ShouldSubscribeSameHandlerOnlyOnce()
    {
      var eventAggregator = new EventAggregator();

      int numberOfCalls = 0;
      EventHandler<int> myHandler = (s, e) => numberOfCalls++;
      eventAggregator.Subscribe<int>(myHandler);
      eventAggregator.Subscribe<int>(myHandler);
      eventAggregator.Subscribe<int>(myHandler);
      eventAggregator.Publish<int>(5);

      Assert.Equal(1, numberOfCalls);
    }

    [Fact]
    public void ShouldSubscribeMultipleHandlers()
    {
      var eventAggregator = new EventAggregator();

      int? receivedValue1 = null;
      int? receivedValue2 = null;
      bool shouldBeFalse = false;
      EventHandler<int> myHandler1 = (s, e) => receivedValue1 = e;
      EventHandler<int> myHandler2 = (s, e) => receivedValue2 = e;
      EventHandler<double> myHandler3 = (s, e) => shouldBeFalse = true;
      eventAggregator.Subscribe<int>(myHandler1);
      eventAggregator.Subscribe<int>(myHandler2);
      eventAggregator.Subscribe<double>(myHandler3);
      eventAggregator.Publish<int>(5);

      Assert.NotNull(receivedValue1);
      Assert.Equal(5, receivedValue1.Value);
      Assert.NotNull(receivedValue2);
      Assert.Equal(5, receivedValue2.Value);
      Assert.False(shouldBeFalse);
    }

    [Fact]
    public void ShouldUnsubscribeFromEvent()
    {
      var eventAggregator = new EventAggregator();

      int? receivedValue1 = null;
      int? receivedValue2 = null;
      EventHandler<int> myHandler1 = (s, e) => receivedValue1 = e;
      EventHandler<int> myHandler2 = (s, e) => receivedValue2 = e;
      eventAggregator.Subscribe<int>(myHandler1);
      eventAggregator.Subscribe<int>(myHandler2);
      eventAggregator.Unsubscribe<int>(myHandler1);
      eventAggregator.Publish<int>(5);

      Assert.Null(receivedValue1);
      Assert.NotNull(receivedValue2);
      Assert.Equal(5, receivedValue2.Value);
    }

    [Fact]
    public void ShouldNotThrownWhenUnsubscribingUnregisteredHandler()
    {
      var eventAggregator = new EventAggregator();
      EventHandler<int> myHandler1 = (s, e) => { };
      eventAggregator.Unsubscribe<int>(myHandler1);
    }

  }
}
