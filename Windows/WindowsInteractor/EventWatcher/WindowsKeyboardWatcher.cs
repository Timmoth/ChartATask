using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Interactors;
using ChartATask.Core.Interactors.EventWatchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsKeyboardWatcher : IKeyboardWatcher
    {
        public event EventHandler<IEvent> OnEvent;

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