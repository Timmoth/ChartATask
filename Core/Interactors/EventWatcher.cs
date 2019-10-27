using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ChartATask.Core.Interactors.EventWatchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors
{
    public abstract class EventWatcher : IWatcher
    {
        private readonly ConcurrentQueue<IEvent> _eventQueue;
        private readonly IKeyboardWatcher _keyboardWatcher;
        private readonly IAppWatcher _appWatcher;

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
            _keyboardWatcher?.Stop();
            _appWatcher?.Stop();
        }

        public void Dispose()
        {
            Stop();
            _keyboardWatcher?.Dispose();
            _appWatcher?.Dispose();
        }

        private void OnWatcherEvent(object sender, IEvent newEvent)
        {
            _eventQueue.Enqueue(newEvent);
        }
    }
}