// ***********************************************************************
// ⚡ MvvmGen => https://github.com/thomasclaudiushuber/mvvmgen
// Copyright © by Thomas Claudius Huber
// Licensed under the MIT license => See the LICENSE file in project root
// ***********************************************************************

namespace MvvmGen.Events
{
    public interface IEventAggregator
    {
        void Publish<TEvent>(TEvent eventToPublish);
        void RegisterSubscriber<TSubscriber>(TSubscriber subscriber);
    }
}