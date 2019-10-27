using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Interactors;

namespace ChartATask.Core.Models.Conditions
{
    public class AndCondition : ICondition
    {
        private readonly List<ICondition> _conditions;

        public AndCondition(List<ICondition> conditions)
        {
            _conditions = conditions;
        }

        public bool Check(ISystemEvaluator evaluator)
        {
            return _conditions.All(condition => condition.Check(evaluator));
        }

        public override string ToString()
        {
            return $"AND {{{string.Join(" && ", _conditions)}}}";
        }
    }
}