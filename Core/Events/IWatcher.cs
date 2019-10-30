using System;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Events
{
    public interface IWatcher : IDisposable
    {
        void Start();
        void Stop();
    }

    public interface IWatcher<TEvent> : IWatcher where TEvent : IEvent
    {
        event EventHandler<TEvent> OnEvent;
    }
}