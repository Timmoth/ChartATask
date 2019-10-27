using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Models
{
    public class AppSessionDataSource : IDataSource<DurationOverTime>
    {
        private DateTime _startTime;
        public List<Trigger> Triggers { get; }
        public event EventHandler<DurationOverTime> OnNewDataPoint;
        public AppSessionDataSource(string appName)
        {
            Triggers = new List<Trigger>
            {
                new Trigger(new List<IEvent>{new AppOpenEvent(appName.ToLower()) }, new AlwaysTrue()),
                new Trigger(new List<IEvent>{new AppCloseEvent(appName.ToLower()) }, new AlwaysTrue())
            };

            _startTime = DateTime.MinValue;
        }

        public void Trigger(IEvent newEvent, ISystemEvaluator evaluator)
        {
            var activatedTriggers = Triggers.Where(trigger => trigger.Events.Contains(newEvent));
            if (!activatedTriggers.Any(trigger => trigger.Condition.Check(evaluator)))
            {
                return;
            }

            if (newEvent is AppOpenEvent appOpenEvent)
            {
                _startTime = DateTime.Now;

            }else if (newEvent is AppCloseEvent appCloseEvent)
            {
                if(_startTime != DateTime.MinValue)
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