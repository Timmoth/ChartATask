using System.Collections.Concurrent;
using System.Collections.Generic;
using ChartATask.Core.Interactors.Watchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors
{
    public abstract class EventWatcher : IWatcher
    {
        private readonly IAppWatcher _appWatcher;
        private readonly ConcurrentQueue<IEvent> _eventQueue;
        private readonly IKeyboardWatcher _keyboardWatcher;

        protected EventWatcher(IKeyboardWatcher keyboardWatcher, IAppWatcher appWatcher)
        {
            _eventQueue = new ConcurrentQueue<IEvent>();
            _keyboardWatcher = keyboardWatcher;
            _keyboardWatcher.OnEvent += OnWatcherEvent;

            _appWatcher = appWatcher;
            _appWatcher.OnEvent += OnWatcherEvent;
        }

        public void SetListeners(List<IEvent> events)
        {
            _keyboardWatcher?.SetListeners(events);
            _appWatcher?.SetListeners(events);
        }

        public void Start()
        {
            _keyboardWatcher?.Start();
            _appWatcher?.Start();
        }

        public void Stop()
        {
            _keyboardWatcher?.Stop();
            _appWatcher?.Stop();
        }

        public void Dispose()
        {
            Stop();
            _keyboardWatcher?.Dispose();
            _appWatcher?.Dispose();
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

        private void OnWatcherEvent(object sender, IEvent newEvent)
        {
            _eventQueue.Enqueue(newEvent);
        }
    }
}