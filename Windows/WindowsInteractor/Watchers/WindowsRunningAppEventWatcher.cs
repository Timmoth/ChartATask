using System;
using System.Management;
using ChartATask.Core.Events;
using ChartATask.Core.Events.AppEvents;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsRunningAppEventWatcher : IEventWatcher
    {
        private readonly ManagementEventWatcher _processStartEvent;
        private readonly ManagementEventWatcher _processStopEvent;

        public WindowsRunningAppEventWatcher()
        {
            _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            _processStartEvent.EventArrived += ProcessStarted;

            _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            _processStopEvent.EventArrived += ProcessStopped;
        }

        public event EventHandler<IEvent> OnEvent;

        public void Start()
        {
            try
            {
                _processStartEvent?.Start();
                _processStopEvent?.Start();
            }
            catch
            {
                // ignored
                Console.WriteLine("Could not start WMI Event Watcher. Must be run in Administrator mode");
            }
        }

        public void Stop()
        {
            try
            {
                _processStartEvent?.Stop();
                _processStopEvent?.Stop();
            }
            catch
            {
                // ignored
            }
        }

        public string EventSocketName => "AppRunningSocket";

        public void Dispose()
        {
            try
            {
                _processStartEvent?.Dispose();
                _processStopEvent?.Dispose();
            }
            catch
            {
                // ignored
            }
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());
            OnEvent?.Invoke(this, new AppRunEvent(processName, true));
        }

        private void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());
            OnEvent?.Invoke(this, new AppRunEvent(processName, false));
        }

        private static string ProcessNameToString(string processName)
        {
            return processName.ToLower().Split('.')[0];
        }
    }
}