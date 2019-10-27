using System;
using System.Collections.Generic;
using System.Text;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors
{
    public interface IWatcher : IDisposable
    {
        void SetListeners(List<IEvent> events);
        void Start();
        void Stop();
    }
}
