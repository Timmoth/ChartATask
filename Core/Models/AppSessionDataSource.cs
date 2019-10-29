using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.AppEvents;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public class AppSessionDataSource : IDataSource<DurationOverTime>
    {
        private DateTime _startTime;

        public AppSessionDataSource(string appName, string appTitle)
        {
            Triggers = new List<Trigger>
            {
                new Trigger(new List<IEvent> {new AppTitleEvent(appName, appTitle, true)}, new AlwaysTrue()),
                new Trigger(new List<IEvent> {new AppTitleEvent(appName, appTitle, false)}, new AlwaysTrue())
            };

            _startTime = DateTime.MinValue;
        }

        public List<Trigger> Triggers { get; }
        public event EventHandler<DurationOverTime> OnNewDataPoint;

        public void Trigger(IEvent newEvent, RequestEvaluator evaluator)
        {
            var activatedTriggers = Triggers.Where(trigger => trigger.Events.Contains(newEvent));
            if (!activatedTriggers.Any(trigger => trigger.Condition.Check(evaluator)))
            {
                return;
            }

            if (Triggers[0].Events.Contains(newEvent) && Triggers[1].Condition.Check(evaluator))
            {
                _startTime = DateTime.Now;
            }
            else if (Triggers[1].Events.Contains(newEvent) && Triggers[1].Condition.Check(evaluator))
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