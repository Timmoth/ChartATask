using System;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events.KeyboardEvents;
using ChartATask.Interactors.Windows.Events.Hooks;

namespace ChartATask.Interactors.Windows.Events
{
    public class WindowsKeyboardWatcher : IKeyboardWatcher
    {
        public event EventHandler<KeyPressedEvent> OnEvent;

        public void Start()
        {
            KeyboardHook.Start();
            KeyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
        }

        public void Stop()
        {
            KeyboardHook.End();
        }

        public void Dispose()
        {
            Stop();
        }

        private void Tracker_OnHookEvent(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine(idObject);
        }

        private void KeyboardHook_OnKeyPressed(int keyCode)
        {
            OnEvent?.Invoke(this, new KeyPressedEvent(keyCode));
        }
    }
}