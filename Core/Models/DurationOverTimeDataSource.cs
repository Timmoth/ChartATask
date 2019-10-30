using System;
using System.Collections.Generic;
using ChartATask.Core.Events;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public class DurationOverTimeDataSource : IDataSource<DurationOverTime>
    {
        private readonly IEnumerable<Trigger> _endTriggers;
        private readonly IEnumerable<Trigger> _startTriggers;

        private RequestEvaluatorManager _evaluator;
        private EventWatcherManager _eventWatcherManager;
        private DateTime _startTime;

        public DurationOverTimeDataSource(
            IEnumerable<Trigger> startTriggers,
            IEnumerable<Trigger> endTriggers)
        {
            _startTriggers = startTriggers;
            _endTriggers = endTriggers;
            _startTime = DateTime.Now;
        }

        public event EventHandler<DurationOverTime> OnNewDataPoint;

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
                var eventWatcher = _eventWatcherManager.GetWatcher(startTrigger.EventSocket.ToString());
                eventWatcher.OnEvent += (s, e) =>
                {
                    if (startTrigger.IsTriggered(e))
                    {
                        _startTime = DateTime.Now;
                    }
                };
            }
        }

        private void SetupEndTasks()
        {
            foreach (var endTrigger in _endTriggers)
            {
                endTrigger.Setup(_evaluator);

                var eventWatcher = _eventWatcherManager.GetWatcher(endTrigger.EventSocket.ToString());
                eventWatcher.OnEvent += (s, e) =>
                {
                    if (!endTrigger.IsTriggered(e) || _startTime == DateTime.MinValue)
                    {
                        return;
                    }

                    var dataPoint = new DurationOverTime(_startTime, DateTime.Now - _startTime);
                    Console.WriteLine(dataPoint);
                    OnNewDataPoint?.Invoke(this, dataPoint);
                    _startTime = DateTime.MinValue;
                };
            }
        }
    }
}