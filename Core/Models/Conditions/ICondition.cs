using ChartATask.Core.Interactors;

namespace ChartATask.Core.Models.Conditions
{
    public interface ICondition
    {
        bool Check(ISystemEvaluator evaluator);
    }
}