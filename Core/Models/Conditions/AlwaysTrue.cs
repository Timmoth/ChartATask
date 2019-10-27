using ChartATask.Core.Interactors;

namespace ChartATask.Core.Models.Conditions
{
    public class AlwaysTrue : ICondition
    {
        public bool Check(ISystemEvaluator evaluator)
        {
            return true;
        }

        public override string ToString()
        {
            return "Always True";
        }
    }
}