using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Events;
using ChartATask.Core.Events.Watchers;
using ChartATask.Core.Models.Acceptor;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.AppEvents;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public class AppSessionDataSource : IDataSource<DurationOverTime>
    {
        private readonly List<Trigger<AppTitleEvent>> _endTriggers;
        private readonly List<Trigger<AppTitleEvent>> _startTriggers;

        private RequestEvaluator _evaluator;
        private DateTime _startTime;
        private List<IWatcher<AppTitleEvent>> _titleWatchers;

        public AppSessionDataSource()
        {
            _startTriggers = new List<Trigger<AppTitleEvent>>();
            _endTriggers = new List<Trigger<AppTitleEvent>>();
            _startTriggers.Add(new Trigger<AppTitleEvent>(new IEventSocket<AppTitleEvent>[]
            {
                new AppTitleEventSocket(new StringContains("application"), new StringContains("Calculator"),
                    new BoolEquality(true))
            }));
            _endTriggers.Add(new Trigger<AppTitleEvent>(new IEventSocket<AppTitleEvent>[]
            {
                new AppTitleEventSocket(new StringContains("application"), new StringContains("Calculator"),
                    new BoolEquality(false))
            }));

            _startTime = DateTime.MinValue;
        }

        public event EventHandler<DurationOverTime> OnNewDataPoint;

        public void Setup(EventWatchers eventWatchers, RequestEvaluator requestEvaluator)
        {
            _evaluator = requestEvaluator;

            _titleWatchers = eventWatchers.GetWatcher<AppTitleEvent>().ToList();
            foreach (var titleWatcher in _titleWatchers)
            {
                titleWatcher.OnEvent += TitleWatcher_OnEvent;
            }
        }

        private void TitleWatcher_OnEvent(object sender, AppTitleEvent e)
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

        public override string ToString()
        {
            return @"Key Pressed Source";
        }
    }
}