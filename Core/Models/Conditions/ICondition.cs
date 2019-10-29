using ChartATask.Core.Requests;

namespace ChartATask.Core.Models.Conditions
{
    public interface ICondition
    {
        bool Check(RequestEvaluator evaluator);
    }
}