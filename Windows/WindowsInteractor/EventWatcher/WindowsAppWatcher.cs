using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using ChartATask.Core.Interactors.EventWatchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsAppWatcher : IAppWatcher
    {
        public event EventHandler<IEvent> OnEvent;
        private readonly List<AppOpenEvent> _appOpenEvents;
        private readonly List<AppCloseEvent> _appCloseEvents;
        private readonly ManagementEventWatcher _processStartEvent;
        private readonly ManagementEventWatcher _processStopEvent;

        public WindowsAppWatcher()
        {
            _appOpenEvents = new List<AppOpenEvent>();
            _appCloseEvents = new List<AppCloseEvent>();

            _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            _processStartEvent.EventArrived += processStartEvent_EventArrived;

            _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            _processStopEvent.EventArrived += processStopEvent_EventArrived;

        }
        
        public void SetListeners(List<IEvent> events)
        {
            _appOpenEvents.AddRange(events.OfType<AppOpenEvent>());
            _appCloseEvents.AddRange(events.OfType<AppCloseEvent>());
        }

        public void Start()
        {
            try
            {
                _processStartEvent?.Start();
                _processStopEvent?.Start();
            }
            catch(Exception exception)
            {

            }

        }

        public void Stop()
        {
            _processStartEvent?.Stop();
            _processStopEvent?.Stop();
        }
        public void Dispose()
        {
            _processStartEvent?.Dispose();
            _processStopEvent?.Dispose();
        }
        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            string processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();

            var triggeredEvent = _appOpenEvents.FirstOrDefault(openEvent => processName.ToLower().Contains(openEvent.Name));
            if (triggeredEvent != null)
            {
                OnEvent?.Invoke(this, triggeredEvent);
            }
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string processName = e.NewEvent.Properties["ProcessName"].Value.ToString();
            string processID = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value).ToString();

            
            var triggeredEvent = _appCloseEvents.FirstOrDefault(closeEvent => processName.ToLower().Contains(closeEvent.Name));
            if (triggeredEvent != null)
            {
                OnEvent?.Invoke(this, triggeredEvent);
            }
        }
    }
}
