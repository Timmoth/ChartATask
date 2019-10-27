using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Models
{
    public class KeyPressedDataSource : IDataSource<IntOverTime>
    {
        public List<Trigger> Triggers { get; }
        public event EventHandler<IntOverTime> OnNewDataPoint;

        public KeyPressedDataSource()
        {
            var allKeys = new List<IEvent>();
            for(int i = 0; i < 255; i++)
            {
                allKeys.Add(new KeyPressedEvent(i));
            }


            Triggers = new List<Trigger>()
            {
                new Trigger(allKeys, new AlwaysTrue()),
            };
        }

        public void Trigger(IEvent newEvent, ISystemEvaluator evaluator)
        {
            var activatedTriggers = Triggers.Where(trigger => trigger.Events.Contains(newEvent));
            if (!activatedTriggers.Any(trigger => trigger.Condition.Check(evaluator)))
            {
                return;
            }

            var value = 0;
            if(newEvent is KeyPressedEvent keyPressEvent)
            {
                value = keyPressEvent.KeyCode;
            }

            OnNewDataPoint?.Invoke(this, new IntOverTime(DateTime.Now, value));
        }

        public override string ToString()
        {
            return $@"Key Pressed Source";
        }
    }
}