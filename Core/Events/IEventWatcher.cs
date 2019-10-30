using System;

namespace ChartATask.Core.Events
{
    public interface IEventWatcher : IDisposable
    {
        string EventSocketName { get; }
        void Start();
        void Stop();

        event EventHandler<IEvent> OnEvent;
    }
}