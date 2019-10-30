using System;
using System.Runtime.InteropServices;

namespace ChartATask.Interactors.Windows.Watchers.Hooks
{
    public delegate void HookEvent(IntPtr hWinEventHook, uint eventType,
        IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    internal class WinEventHook : IDisposable
    {
        public static readonly uint EVENT_OBJECT_FOCUS = 0x8005;
        public static readonly uint EVENT_OBJECT_CREATE = 0x8000;
        public static readonly uint EVENT_SYSTEM_FOREGROUND = 3;
        public static readonly uint EVENT_OBJECT_NAMECHANGE = 0x800C;
        public static readonly uint WINEVENT_OUTOFCONTEXT = 0;
        private readonly WinEventDelegate _callback;

        private readonly IntPtr _hookId;

        public WinEventHook(uint eventCode, uint idProcess)
        {
            _callback = WinEventProc;
            _hookId = SetWinEventHook(
                eventCode,
                eventCode,
                IntPtr.Zero,
                _callback,
                idProcess,
                0,
                WINEVENT_OUTOFCONTEXT);
        }

        public void Dispose()
        {
            try
            {
                UnhookWinEvent(_hookId);
            }
            catch
            {
                // ignored
            }
        }

        public event HookEvent OnHookEvent;

        private void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime)
        {
            OnHookEvent?.Invoke(hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
                hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
    }
}