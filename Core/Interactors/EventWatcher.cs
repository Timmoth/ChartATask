using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ChartATask.Core.Interactors.EventWatchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Interactors
{
    public abstract class EventWatcher : IDisposable
    {
        private readonly ConcurrentQueue<IEvent> _eventQueue;
        private readonly IKeyboardWatcher _keyboardWatcher;

        protected EventWatcher(IKeyboardWatcher keyboardWatcher)
        {
            _eventQueue = new ConcurrentQueue<IEvent>();
            _keyboardWatcher = keyboardWatcher;
            _keyboardWatcher.OnEvent += OnWatcherEvent;
        }

        public void SetListeners(List<IEvent> events)
        {

        }

        public void Start()
        {
            _keyboardWatcher?.Start();
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
        }

        public void Dispose()
        {
            Stop();
            _keyboardWatcher?.Dispose();
        }

        private void OnWatcherEvent(object sender, IEvent newEvent)
        {
            _eventQueue.Enqueue(newEvent);
        }
    }
}