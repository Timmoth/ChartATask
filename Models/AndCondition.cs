using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models
{
    public class AndCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public AndCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Passed(IConditionEvaluator evaluator)
        {
            return _conditions.All(condition => condition.Passed(evaluator));
        }
    }
}