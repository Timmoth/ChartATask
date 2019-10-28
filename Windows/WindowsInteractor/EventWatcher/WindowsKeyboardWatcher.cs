using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Interactors.Watchers;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.KeyboardEvents;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsKeyboardWatcher : IKeyboardWatcher
    {
        private readonly Thread _keyboardHookThread;
        private bool _isRunning;

        public WindowsKeyboardWatcher()
        {
            _keyboardHookThread = new Thread(() =>
            {
                _isRunning = true;

                KeyboardHook.OnKeyPressed += KeyboardHook_OnKeyPressed;
                KeyboardHook.Start();

                while (_isRunning)
                {
                    Task.Delay(1).Wait();
                }

                KeyboardHook.End();
            });
        }

        public event EventHandler<IEvent> OnEvent;

        public void SetListeners(List<IEvent> events)
        {
        }

        public void Start()
        {
            _keyboardHookThread?.Start();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Dispose()
        {
            Stop();
            _keyboardHookThread?.Abort();
        }

        private void KeyboardHook_OnKeyPressed(int keyCode)
        {
            OnEvent?.Invoke(this, new KeyPressedEvent(keyCode));
        }
    }
}