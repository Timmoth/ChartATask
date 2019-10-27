using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models.Conditions
{
    public class AndCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public AndCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Check(ISystem evaluator)
        {
            return _conditions.All(condition => condition.Check(evaluator));
        }
    }
}