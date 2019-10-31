using ChartATask.Core.Triggers.Requests;

namespace ChartATask.Core.Triggers.Conditions
{
    public interface ICondition
    {
        bool Check();
        void Setup(RequestEvaluatorManager requestEvaluatorManager);
    }
}