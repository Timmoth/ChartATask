using System;

namespace ChartATask.Interactors.Windows.Watchers.Hooks
{
    public delegate void HookEvent(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
}