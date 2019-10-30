using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Events
{
    public class EventWatcherManager
    {
        private readonly List<IEventWatcher> _eventWatchers;

        public EventWatcherManager()
        {
            _eventWatchers = new List<IEventWatcher>();
        }

        public void Register<TEvent>(IEventWatcher<TEvent> eventEventWatcher) where TEvent : IEvent
        {
            _eventWatchers.Add(eventEventWatcher);
        }

        public IEnumerable<IEventWatcher<T>> GetWatcher<T>() where T : IEvent
        {
            return _eventWatchers.OfType<IEventWatcher<T>>();
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
    }
}