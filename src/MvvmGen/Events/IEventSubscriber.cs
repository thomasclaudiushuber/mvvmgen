// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.Events
{
  public interface IEventSubscriber<TEvent>
  {
    void OnEvent(TEvent eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2> : IEventSubscriber<TEvent1>
  {
    void OnEvent(TEvent2 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3> : IEventSubscriber<TEvent1, TEvent2>
  {
    void OnEvent(TEvent3 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4> : IEventSubscriber<TEvent1, TEvent2, TEvent3>
  {
    void OnEvent(TEvent4 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4>
  {
    void OnEvent(TEvent5 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5>
  {
    void OnEvent(TEvent6 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6>
  {
    void OnEvent(TEvent7 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7>
  {
    void OnEvent(TEvent8 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8>
  {
    void OnEvent(TEvent9 eventData);
  }

  public interface IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9, TEvent10> : IEventSubscriber<TEvent1, TEvent2, TEvent3, TEvent4, TEvent5, TEvent6, TEvent7, TEvent8, TEvent9>
  {
    void OnEvent(TEvent10 eventData);
  }
}
