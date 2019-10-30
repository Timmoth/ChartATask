using ChartATask.Core.Requests;

namespace ChartATask.Core.Models.Conditions
{
    public interface ICondition
    {
        bool Check();
        void Setup(RequestEvaluatorManager requestEvaluatorManager);
    }
}