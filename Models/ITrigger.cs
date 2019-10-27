using System.Collections.Generic;
using ChartATask.Models.Conditions;
using ChartATask.Models.Events;

namespace ChartATask.Models
{
    public class Trigger
    {
        public List<IEvent> Events { get; }
        public ICondition Condition { get; }

        public Trigger(List<IEvent> events, ICondition condition)
        {
            Events = events;
            Condition = condition;
        }    
        
        public Trigger(List<IEvent> events) : this(events, new TrueCondition())
        {
        }
    }
}