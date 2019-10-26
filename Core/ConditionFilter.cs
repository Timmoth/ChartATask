using System.Collections.Generic;
using System.Linq;
using ChartATask.Models;

namespace ChartATask.Core
{
    public class WindowsConditionEvaluator : IConditionEvaluator
    {
    }

    internal class ConditionFilter
    {
        private readonly WindowsConditionEvaluator evaluator;

        public ConditionFilter()
        {
            evaluator = new WindowsConditionEvaluator();
        }

        public List<CoreAction> Filter(List<CoreAction> triggeredActions)
        {
            return triggeredActions.Where(triggeredAction =>
                triggeredAction.Conditions.All(condition => condition.Passed(evaluator))).ToList();
        }
    }
}