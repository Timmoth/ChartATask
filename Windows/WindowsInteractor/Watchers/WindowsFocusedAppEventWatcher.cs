using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Events.App;
using ChartATask.Interactors.Windows.Watchers.Hooks;
using System;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsFocusedAppEventWatcher : EventWatcher
    {
        private WinEventHook _eventHook;
        public WindowsFocusedAppEventWatcher() : base("AppFocusSocket")
        {

        }
        public override void Start()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_OBJECT_FOCUS, 0);
            _eventHook.OnHookEvent += OnHookEvent;
        }

        public override void Stop()
        {
            _eventHook?.Dispose();
        }

        public override void Dispose()
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

            Fire(new AppFocusChanged(processName, windowTitle));
        }
    }
}