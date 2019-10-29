using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Events;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{ 
    public class DurationOverTimeDataSource<TEvent> : IDataSource<DurationOverTime> where TEvent : IEvent
    {
        private readonly IEnumerable<Trigger<TEvent>> _startTriggers;
        private readonly IEnumerable<Trigger<TEvent>> _endTriggers;

        private RequestEvaluator _evaluator;
        private DateTime _startTime;
        public event EventHandler<DurationOverTime> OnNewDataPoint;

        public DurationOverTimeDataSource(IEnumerable<Trigger<TEvent>> startTriggers, IEnumerable<Trigger<TEvent>> endTriggers)
        {
            _startTriggers = startTriggers;
            _endTriggers = endTriggers;
            _startTime = DateTime.Now;
        }

        public void Setup(EventWatchers eventWatchers, RequestEvaluator requestEvaluator)
        {
            _evaluator = requestEvaluator;
            foreach (var watcher in eventWatchers.GetWatcher<TEvent>().ToList())
            {
                watcher.OnEvent += ProcessEvent;
            }
        }

        private void ProcessEvent(object sender, TEvent e)
        {
            if (_startTriggers.Any(p => p.IsTriggered(e, _evaluator)))
            {
                _startTime = DateTime.Now;
            }
            else if (_endTriggers.Any(p => p.IsTriggered(e, _evaluator)))
            {
                if (_startTime != DateTime.MinValue)
                {
                    OnNewDataPoint?.Invoke(this, new DurationOverTime(_startTime, DateTime.Now - _startTime));
                }

                _startTime = DateTime.MinValue;
            }
        }
    }
}