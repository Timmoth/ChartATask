using System;
using ChartATask.Core.Data.Points;
using ChartATask.Core.Triggers.Acceptor;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Events.App;
using ChartATask.Core.Triggers.Events.Sockets;
using ChartATask.Core.Triggers.Requests;

namespace ChartATask.Core.Data.Sources
{
    public class SessionDurationSource : IDataSource<SessionDuration>
    {
        private IEventWatcher _focusChangeWatcher;
        private IEventWatcher _titleChangeWatcher;
        private RequestEvaluatorManager _evaluator;
        private EventWatcherManager _eventWatcherManager;
        private string _name;
        private string _title;
        private DateTime _startTime;

        public SessionDurationSource()
        {
            _startTime = DateTime.MinValue;
        }

        public event EventHandler<SessionDuration> OnNewDataPoint;

        public void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator)
        {
            _evaluator = requestEvaluator;
            _eventWatcherManager = eventWatcherManager;
            _focusChangeWatcher = _eventWatcherManager.GetWatcher(new AppFocusSocket(new Always<string>(true), new Always<string>(true)));
            _titleChangeWatcher = _eventWatcherManager.GetWatcher(new AppTitleSocket(new Always<string>(true), new Always<string>(true)));
            _focusChangeWatcher.OnEvent += _focusChangeWatcher_OnEvent;
            _titleChangeWatcher.OnEvent += _titleChangeWatcher_OnEvent; ;
        }

        private void _titleChangeWatcher_OnEvent(object sender, IEvent e)
        {
            if (e is AppTitleChanged appTitleChanged && appTitleChanged.Name == _name)
            {
                TryAddDataPoint(appTitleChanged.Name, appTitleChanged.Title);
            }
        }

        private void _focusChangeWatcher_OnEvent(object sender, IEvent e)
        {
            if (e is AppFocusChanged appFocusChanged)
            {
                TryAddDataPoint(appFocusChanged.Name, appFocusChanged.Title);
            }
        }

        private void TryAddDataPoint(string name, string title)
        {
            if (!string.IsNullOrEmpty(_name) && _name != name && _title != title)
            {
                OnNewDataPoint?.Invoke(this, new SessionDuration(_name, _title, _startTime, DateTime.Now));
            }

            _name = name;
            _title = title;
            _startTime = DateTime.Now;
        }
    }
}