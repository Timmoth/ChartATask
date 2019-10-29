using ChartATask.Core.Models.Conditions;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public class Trigger<TEvent> where TEvent : IEvent
    {
        private readonly IEventSocket<TEvent> _eventSocket;
        private readonly ICondition _condition;
        public Trigger(IEventSocket<TEvent> eventSocket, ICondition condition)
        {
            _eventSocket = eventSocket;
            _condition = condition;
        }

        public Trigger(IEventSocket<TEvent> eventSocket) : this(eventSocket, new AlwaysTrue())
        {
        }

        public bool IsTriggered(TEvent firedEvent, RequestEvaluator evaluator)
        {
            return _eventSocket.Accepts(firedEvent) && _condition.Check(evaluator);
        }

        public override string ToString()
        {
            return $"Trigger: \n\tCondition:\n\t\t{_condition}\n\tEvents:\n\t\t{string.Join("\n\t\t", _eventSocket)}";
        }
    }
}