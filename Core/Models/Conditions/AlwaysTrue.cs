using ChartATask.Core.Requests;

namespace ChartATask.Core.Models.Conditions
{
    public class AlwaysTrue : ICondition
    {
        public bool Check(RequestEvaluator evaluator)
        {
            return true;
        }

        public override string ToString()
        {
            return "Always True";
        }
    }
}