using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models.Events;

namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsEventWatcher : IEventWatcher
    {
        private readonly ConcurrentQueue<IEvent> _eventQueue;
        private readonly Thread _keyboardHookThread;
        private bool _isRunning;

        public WindowsEventWatcher()
        {
            _eventQueue = new ConcurrentQueue<IEvent>();

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

        public void SetListeners(List<IEvent> events)
        {
        }

        public void Start()
        {
            _keyboardHookThread?.Start();
        }

        public Queue<IEvent> GetEvents()
        {
            var newEvents = new Queue<IEvent>();
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