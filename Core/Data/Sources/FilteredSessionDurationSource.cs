using System;
using System.Collections.Generic;
using ChartATask.Core.Data.Points;
using ChartATask.Core.Triggers;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;

namespace ChartATask.Core.Data.Sources
{
    public class FilteredSessionDurationSource : IDataSource<SessionDuration>
    {
        private readonly IEnumerable<Trigger> _endTriggers;
        private readonly IEnumerable<Trigger> _startTriggers;

        private RequestEvaluatorManager _evaluator;
        private EventWatcherManager _eventWatcherManager;
        private DateTime _startTime;

        public FilteredSessionDurationSource(
            IEnumerable<Trigger> startTriggers,
            IEnumerable<Trigger> endTriggers)
        {
            _startTriggers = startTriggers;
            _endTriggers = endTriggers;
            _startTime = DateTime.MinValue;
        }

        public event EventHandler<SessionDuration> OnNewDataPoint;

        public void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator)
        {
            _evaluator = requestEvaluator;
            _eventWatcherManager = eventWatcherManager;
            SetupStartTasks();
            SetupEndTasks();
        }

        private void SetupStartTasks()
        {
            foreach (var startTrigger in _startTriggers)
            {
                startTrigger.Setup(_evaluator);
                var eventWatcher = _eventWatcherManager.GetWatcher(startTrigger.EventSocket);
                eventWatcher.OnEvent += (s, e) =>
                {
                    if (!startTrigger.IsTriggered(e) || _startTime != DateTime.MinValue)
                    {
                        return;
                    }

                    _startTime = DateTime.Now;
                };
            }
        }

        private void SetupEndTasks()
        {
            foreach (var endTrigger in _endTriggers)
            {
                endTrigger.Setup(_evaluator);
                var eventWatcher = _eventWatcherManager.GetWatcher(endTrigger.EventSocket);
                eventWatcher.OnEvent += (s, e) =>
                {
                    if (!endTrigger.IsTriggered(e) || _startTime == DateTime.MinValue)
                    {
                        return;
                    }

                    var dataPoint = new SessionDuration("","",_startTime, DateTime.Now);
                    _startTime = DateTime.MinValue;
                    OnNewDataPoint?.Invoke(this, dataPoint);
                };
            }
        }
    }
}