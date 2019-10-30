using System;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Interactors.Windows.Events.Hooks
{
    public class WindowsAppTitleWatcher : IAppTitleWatcher
    {
        private WinEventHook _eventHook;
        public event EventHandler<AppTitleEvent> OnEvent;

        public void Dispose()
        {
            _eventHook?.Dispose();
        }

        public void Start()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_OBJECT_NAMECHANGE, 0);
            _eventHook.OnHookEvent += _eventHook_OnHookEvent;
        }

        public void Stop()
        {
        }

        private void _eventHook_OnHookEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
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

            OnEvent?.Invoke(this, new AppTitleEvent(processName, windowTitle, true));
        }
    }
}