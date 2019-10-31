using System;

namespace ChartATask.Core.Triggers.Events
{
    public interface IEventWatcher : IDisposable
    {
        string EventSocketName { get; }
        void Start();
        void Stop();

        event EventHandler<IEvent> OnEvent;
    }
}