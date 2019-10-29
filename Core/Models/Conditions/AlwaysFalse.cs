using ChartATask.Core.Requests;

namespace ChartATask.Core.Models.Conditions
{
    public class AlwaysFalse : ICondition
    {
        public bool Check(RequestEvaluator evaluator)
        {
            return false;
        }

        public override string ToString()
        {
            return "Always False";
        }
    }
}