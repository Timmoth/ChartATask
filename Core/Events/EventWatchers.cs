using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Events
{
    public class EventWatchers
    {
        private readonly List<IWatcher> _eventWatchers;

        public EventWatchers()
        {
            _eventWatchers = new List<IWatcher>();
        }

        public void Register<TEvent>(IWatcher<TEvent> eventWatcher) where TEvent : IEvent
        {
            _eventWatchers.Add(eventWatcher);
        }

        public IEnumerable<IWatcher<T>> GetWatcher<T>() where T : IEvent
        {
            return _eventWatchers.OfType<IWatcher<T>>();
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