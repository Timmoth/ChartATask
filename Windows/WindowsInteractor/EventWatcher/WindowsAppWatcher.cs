using System;
using System.Threading;
using ChartATask.Core.Interactors.EventWatchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsAppWatcher : IAppWatcher
    {
        public event EventHandler<IEvent> OnEvent;

        public WindowsAppWatcher()
        {

        }

        public void Dispose()
        {

        }
        public void Start()
        {

        }

        public void Stop()
        {

        }
    }
}
