using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Events
{
    public class EventWatcherManager
    {
        private readonly List<IEventWatcher> _eventWatchers;

        public EventWatcherManager()
        {
            _eventWatchers = new List<IEventWatcher>();
        }

        public void Register(IEventWatcher eventEventWatcher)
        {
            _eventWatchers.Add(eventEventWatcher);
        }

        public IEventWatcher GetWatcher(string eventSocketName)
        {
            return _eventWatchers.FirstOrDefault(watcher => watcher.EventSocketName == eventSocketName);
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