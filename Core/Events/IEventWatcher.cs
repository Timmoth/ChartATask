using System;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Events
{
    public interface IEventWatcher : IDisposable
    {
        void Start();
        void Stop();
    }

    public interface IEventWatcher<TEvent> : IEventWatcher where TEvent : IEvent
    {
        event EventHandler<TEvent> OnEvent;
    }
}