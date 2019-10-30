using System;
using ChartATask.Core.Events;
using ChartATask.Core.Events.AppEvents;
using ChartATask.Interactors.Windows.Watchers.Hooks;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsAppTitleEventWatcher : IEventWatcher
    {
        private WinEventHook _eventHook;
        public event EventHandler<IEvent> OnEvent;

        public void Dispose()
        {
            _eventHook?.Dispose();
        }

        public void Start()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_OBJECT_NAMECHANGE, 0);
            _eventHook.OnHookEvent += OnHookEvent;
        }

        public void Stop()
        {
            _eventHook?.Dispose();
        }

        public string EventSocketName => "AppTitleSocket";

        private void OnHookEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
            int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var windowTitle = WindowsFunctions.GetWindowTitle(hwnd);
            if (string.IsNullOrEmpty(windowTitle))
            {
                return;
            }

            var processName = WindowsFunctions.GetProcessName(hwnd);
            if (string.IsNullOrEmpty(processName))
            {
                return;
            }

            OnEvent?.Invoke(this, new AppTitleChanged(processName, windowTitle));
        }
    }
}