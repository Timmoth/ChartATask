using System;

namespace ChartATask.Core.Triggers.Events
{
    public abstract class EventWatcher : IDisposable
    {
        public event EventHandler<IEvent> OnEvent;
        public string EventSocketName { get; }

        protected EventWatcher(string eventSocketName)
        {
            EventSocketName = eventSocketName;
        }

        public virtual void Start()
        {

        }
        public virtual void Stop()
        {

        }
        public abstract void Dispose();

        protected void Fire(IEvent newEvent)
        {
            OnEvent?.Invoke(this, newEvent);
        }
    }
}