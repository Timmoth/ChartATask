using ChartATask.Core.Triggers.Conditions;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;

namespace ChartATask.Core.Triggers
{
    public interface ITrigger
    {
        bool IsTriggered(IEvent firedEvent);
    }

    public class Trigger : ITrigger
    {
        private readonly ICondition _condition;

        public Trigger(IEventSocket eventSocket, ICondition condition = null)
        {
            EventSocket = eventSocket;
            _condition = condition;
        }

        public IEventSocket EventSocket { get; }

        public bool IsTriggered(IEvent firedEvent)
        {
            return EventSocket.Accepts(firedEvent) && _condition.Check();
        }

        public override string ToString()
        {
            return $"Trigger: \n\tCondition:\n\t\t{_condition}\n\tEvents:\n\t\t{string.Join("\n\t\t", EventSocket)}";
        }


        public void Setup(RequestEvaluatorManager requestEvaluator)
        {
            _condition.Setup(requestEvaluator);
        }
    }
}