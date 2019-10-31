using ChartATask.Core.Requests;

namespace ChartATask.Core.Conditions
{
    public interface ICondition
    {
        bool Check();
        void Setup(RequestEvaluatorManager requestEvaluatorManager);
    }
}