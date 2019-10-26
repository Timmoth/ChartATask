using System;
using System.Collections.Generic;
using ChartATask.Models;

namespace ChartATask.Interactors
{
    public interface IInteractor : IDisposable
    {
        Queue<IInteractionEvent> GetEvents();
        void SetListeners(List<CoreAction> actionManagerActions);
        void Start();
        void Stop();
    }
}