using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Triggers.Events
{
    public class EventWatcherManager
    {
        private readonly Dictionary<string, IEventWatcher> _eventWatchers;

        public EventWatcherManager(IEnumerable<IEventWatcher> eventWatchers)
        {
            _eventWatchers = new Dictionary<string, IEventWatcher>();
            foreach (var eventWatcher in eventWatchers)
            {
                _eventWatchers.Add(eventWatcher.EventSocketName, eventWatcher);
            }
        }

        public IEventWatcher GetWatcher(IEventSocket socket)
        {
            _eventWatchers.TryGetValue(socket.ToString(), out var watcher);
            return watcher;
        }

        public void Start()
        {
            _eventWatchers.ToList().ForEach(pair => pair.Value.Start());
        }

        public void Stop()
        {
            _eventWatchers.ToList().ForEach(pair => pair.Value.Stop());
        }

        public void Dispose()
        {
            Stop();
            _eventWatchers.ToList().ForEach(pair => pair.Value.Dispose());
        }
    }
}