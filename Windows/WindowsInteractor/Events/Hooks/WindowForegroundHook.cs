using System;

namespace ChartATask.Interactors.Windows.Events.Hooks
{
    internal class WindowForegroundHook : IDisposable
    {
        private readonly WinEventHook _eventHook;

        public WindowForegroundHook()
        {
            _eventHook = new WinEventHook(WinEventHook.EVENT_SYSTEM_FOREGROUND, 0);
            _eventHook.OnHookEvent += _eventHook_OnHookEvent;
        }

        public void Dispose()
        {
            _eventHook?.Dispose();
        }

        private void _eventHook_OnHookEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject,
            int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine("Foreground: " + WindowsFunctions.GetProcessName(hwnd) + " " +
                              WindowsFunctions.GetWindowTitle(hwnd));
        }
    }
}