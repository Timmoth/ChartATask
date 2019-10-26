using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Models;

namespace ChartATask.Interactors.Windows
{
    public class WindowsInteractor : IInteractor
    {
        private readonly ConcurrentQueue<IInteractionEvent> _eventQueue;
        private readonly Thread _keyboardHookThread;
        private bool _isRunning;

        public WindowsInteractor()
        {
            _eventQueue = new ConcurrentQueue<IInteractionEvent>();

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

        public void SetListeners(List<CoreAction> actionManagerActions)
        {
        }

        public void Start()
        {
            _keyboardHookThread?.Start();
        }

        public Queue<IInteractionEvent> GetEvents()
        {
            var newEvents = new Queue<IInteractionEvent>();
            while (_eventQueue.TryDequeue(out var newEvent))
            {
                newEvents.Enqueue(newEvent);
            }

            return newEvents;
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
            _eventQueue.Enqueue(new KeyPressedEvent(keyCode));
        }
    }
}