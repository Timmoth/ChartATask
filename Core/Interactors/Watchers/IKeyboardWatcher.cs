using System;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors.Watchers
{
    public interface IKeyboardWatcher : IWatcher
    {
        event EventHandler<IEvent> OnEvent;
    }
}