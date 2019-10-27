using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models.Conditions
{
    public class OrCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public OrCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Check(IEvaluator evaluator)
        {
            return _conditions.Any(condition => condition.Check(evaluator));
        }
    }
}