using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using ChartATask.Core.Interactors.Watchers;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.AppEvents;
using HWND = System.IntPtr;


namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsAppWatcher : IAppWatcher
    {
        private readonly List<AppCloseEvent> _appCloseEvents;
        private readonly List<AppOpenEvent> _appOpenEvents;
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

        public event EventHandler<IEvent> OnEvent;

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
            catch (Exception exception)
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

        private void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower().Split('.')[0];

            if (_appOpenEvents.Any(openEvent => processName.Equals(openEvent.Name.ToLower())))
            {
                OnEvent?.Invoke(this, new AppOpenEvent(processName));
            }
        }

        private void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var processName = e.NewEvent.Properties["ProcessName"].Value.ToString();

            if (_appCloseEvents.Any(openEvent => processName.Equals(openEvent.Name.ToLower())))
            {
                OnEvent?.Invoke(this, new AppCloseEvent(processName));
            }
        }
    }
}