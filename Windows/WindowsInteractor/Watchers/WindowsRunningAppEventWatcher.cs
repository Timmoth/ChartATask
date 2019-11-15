using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Events.App;
using System;
using System.Globalization;
using System.Management;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsRunningAppEventWatcher : EventWatcher
    {
        private readonly ManagementEventWatcher _processStartEvent;
        private readonly ManagementEventWatcher _processStopEvent;

        public WindowsRunningAppEventWatcher() : base("AppRunningSocket")
        {
            _processStartEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStartTrace");
            _processStartEvent.EventArrived += ProcessStarted;

            _processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            _processStopEvent.EventArrived += ProcessStopped;
        }

        public override void Start()
        {
            try
            {
                _processStartEvent?.Start();
                _processStopEvent?.Start();
            }
            catch
            {
                // ignored
            }
        }

        public override void Stop()
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

        public override void Dispose()
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
            Fire(new AppRunEvent(processName, true));
        }

        private void ProcessStopped(object sender, EventArrivedEventArgs e)
        {
            var processName = ProcessNameToString(e.NewEvent.Properties["ProcessName"].Value.ToString());
            Fire(new AppRunEvent(processName, false));
        }

        private static string ProcessNameToString(string processName)
        {
            return processName.ToLower(CultureInfo.InvariantCulture).Split('.')[0];
        }
    }
}