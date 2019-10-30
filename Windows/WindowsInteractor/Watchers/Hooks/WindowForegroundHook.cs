using System;

namespace ChartATask.Interactors.Windows.Watchers.Hooks
{
    internal class WindowForegroundHook : IDisposable
    {
        private readonly WinEventHook _eventHook;

        public WindowForegroundHook()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_SYSTEM_FOREGROUND, 0);
            _eventHook.OnHookEvent += OnHookEvent;
        }

        public void Dispose()
        {
            _eventHook?.Dispose();
        }

        private void OnHookEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
            int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine("Foreground: " + WindowsFunctions.GetProcessName(hwnd) + " " +
                              WindowsFunctions.GetWindowTitle(hwnd));
        }
    }
}