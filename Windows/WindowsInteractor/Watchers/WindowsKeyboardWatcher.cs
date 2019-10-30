using System;
using ChartATask.Core.Events;
using ChartATask.Core.Models.Events.KeyboardEvents;
using ChartATask.Interactors.Windows.Watchers.Hooks;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsKeyboardWatcher : IWatcher<KeyPressedEvent>
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