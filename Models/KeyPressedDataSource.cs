using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Models.Conditions;
using ChartATask.Models.Events;

namespace ChartATask.Models
{
    public class KeyPressedDataSource : IDataSource<IntOverTime>
    {
        public List<Trigger> Triggers { get; }
        public event EventHandler<IntOverTime> OnNewDataPoint;

        public KeyPressedDataSource()
        {
            Triggers = new List<Trigger>()
            {
                new Trigger(new List<IEvent>()
                {
                    new KeyPressedEvent(65),
                    new KeyPressedEvent(66),
                }, new TrueCondition())

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