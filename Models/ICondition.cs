namespace ChartATask.Models
{
    public interface ICondition
    {
        bool Passed(IConditionEvaluator evaluator);
    }
}