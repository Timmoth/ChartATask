using System;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors.EventWatchers
{
    public interface IAppWatcher : IWatcher
    {
        event EventHandler<IEvent> OnEvent;
    }
}
