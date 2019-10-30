using System;
using ChartATask.Core.Events;
using ChartATask.Core.Models.Events.AppEvents;
using ChartATask.Interactors.Windows.Watchers.Hooks;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsFocusedAppEventWatcher : IEventWatcher<AppFocusChanged>
    {
        private WinEventHook _eventHook;
        public event EventHandler<AppFocusChanged> OnEvent;

        public void Start()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_OBJECT_FOCUS, 0);
            _eventHook.OnHookEvent += OnHookEvent;
        }

        public void Stop()
        {
            _eventHook?.Dispose();
        }

        public void Dispose()
        {
            _eventHook?.Dispose();
        }

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

            OnEvent?.Invoke(this, new AppFocusChanged(processName, windowTitle));
        }
    }
}