using System.Collections.Generic;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Models
{
    public class Trigger
    {
        public Trigger(List<IEvent> events, ICondition condition)
        {
            Events = events;
            Condition = condition;
        }

        public Trigger(List<IEvent> events) : this(events, new AlwaysTrue())
        {
        }

        public List<IEvent> Events { get; }
        public ICondition Condition { get; }

        public override string ToString()
        {
            return $"Trigger: \n\tCondition:\n\t\t{Condition}\n\tEvents:\n\t\t{string.Join("\n\t\t", Events)}";
        }
    }
}