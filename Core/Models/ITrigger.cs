using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public class Trigger<TEvent> where TEvent : IEvent
    {
        public Trigger(IEnumerable<IEventSocket<TEvent>> eventSockets, ICondition condition)
        {
            EventSockets = eventSockets.ToList();
            Condition = condition;
        }

        public Trigger(IEnumerable<IEventSocket<TEvent>> eventSockets) : this(eventSockets, new AlwaysTrue())
        {
        }

        public List<IEventSocket<TEvent>> EventSockets { get; }
        public ICondition Condition { get; }

        public bool IsTriggered(TEvent firedEvent, RequestEvaluator evaluator)
        {
            return EventSockets.Any(socket => socket.Accepts(firedEvent)) && Condition.Check(evaluator);
        }

        public override string ToString()
        {
            return $"Trigger: \n\tCondition:\n\t\t{Condition}\n\tEvents:\n\t\t{string.Join("\n\t\t", EventSockets)}";
        }
    }
}