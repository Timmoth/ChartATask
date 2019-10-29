using System.Collections.Concurrent;
using System.Collections.Generic;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Events
{
    public class EventCollector
    {
        private readonly ConcurrentQueue<IEvent> _eventQueue;
        private readonly List<IWatcher> _eventWatchers;

        public EventCollector()
        {
            _eventQueue = new ConcurrentQueue<IEvent>();
            _eventWatchers = new List<IWatcher>();
        }

        public void Register<TEvent>(IWatcher<TEvent> eventWatcher) where TEvent : IEvent
        {
            _eventWatchers.Add(eventWatcher);
            eventWatcher.OnEvent += (s, e) => { _eventQueue.Enqueue(e); };
        }

        public void Start()
        {
            foreach (var eventWatcher in _eventWatchers)
            {
                eventWatcher.Start();
            }
        }

        public void Stop()
        {
            foreach (var eventWatcher in _eventWatchers)
            {
                eventWatcher.Stop();
            }
        }

        public void Dispose()
        {
            Stop();
            foreach (var eventWatcher in _eventWatchers)
            {
                eventWatcher.Dispose();
            }
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
    }
}