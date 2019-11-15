using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Events.Keyboard;
using ChartATask.Interactors.Windows.Watchers.Hooks;
using System;

namespace ChartATask.Interactors.Windows.Watchers
{
    public class WindowsKeyboardEventWatcher : EventWatcher
    {
        public WindowsKeyboardEventWatcher() : base("KeyPressedSocket")
        {

        }
        public override void Start()
        {
            KeyboardHook.Start();
            KeyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
        }

        public override void Stop()
        {
            KeyboardHook.End();
        }

        public override void Dispose()
        {
            Stop();
        }

        private void KeyboardHook_OnKeyPressed(int keyCode)
        {
            Fire(new KeyPressedEvent(keyCode));
        }
    }
}