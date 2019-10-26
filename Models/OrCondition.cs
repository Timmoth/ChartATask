using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models
{
    public class OrCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public OrCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Passed(IConditionEvaluator evaluator)
        {
            return _conditions.Any(condition => condition.Passed(evaluator));
        }
    }
}