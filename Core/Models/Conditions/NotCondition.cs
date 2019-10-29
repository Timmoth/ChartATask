using ChartATask.Core.Requests;

namespace ChartATask.Core.Models.Conditions
{
    public class NotCondition : ICondition
    {
        private readonly ICondition _condition;

        public NotCondition(ICondition condition)
        {
            _condition = condition;
        }

        public bool Check(RequestEvaluator evaluator)
        {
            return !_condition.Check(evaluator);
        }

        public override string ToString()
        {
            return $@"Not {{{_condition}}}";
        }
    }
}