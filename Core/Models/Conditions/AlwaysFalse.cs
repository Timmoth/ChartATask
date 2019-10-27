using ChartATask.Core.Interactors;

namespace ChartATask.Core.Models.Conditions
{
    public class AlwaysFalse : ICondition
    {
        public bool Check(ISystemEvaluator evaluator)
        {
            return false;
        }

        public override string ToString()
        {
            return "Always False";
        }
    }
}