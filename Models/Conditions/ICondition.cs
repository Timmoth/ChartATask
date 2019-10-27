namespace ChartATask.Models.Conditions
{
    public interface ICondition
    {
        bool Check(ISystemEvaluator evaluator);
    }
}