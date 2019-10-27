using System;
using System.Collections.Generic;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors
{
    public interface IEventWatcher : IDisposable
    {
        Queue<IEvent> GetEvents();
        void SetListeners(List<IEvent> eventList);
        void Start();
        void Stop();
    }
}