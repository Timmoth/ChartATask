using System;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors.EventWatchers
{
    public interface IKeyboardWatcher : IDisposable
    {
        event EventHandler<IEvent> OnEvent;
        void Start();
        void Stop();
    }
}
