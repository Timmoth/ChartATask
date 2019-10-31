using System;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Events.Keyboard;
using ChartATask.Interactors.Windows.Watchers.Hooks;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsKeyboardEventWatcher : IEventWatcher
    {
        public event EventHandler<IEvent> OnEvent;

        public void Start()
        {
            KeyboardHook.Start();
            KeyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
        }

        public void Stop()
        {
            KeyboardHook.End();
        }

        public string EventSocketName => "KeyPressedSocket";

        public void Dispose()
        {
            Stop();
        }

        private void KeyboardHook_OnKeyPressed(int keyCode)
        {
            OnEvent?.Invoke(this, new KeyPressedEvent(keyCode));
        }
    }
}