using System;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events.KeyboardEvents;

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

        private void KeyboardHook_OnKeyPressed(int keyCode)
        {
            OnEvent?.Invoke(this, new KeyPressedEvent(keyCode));
        }
    }
}