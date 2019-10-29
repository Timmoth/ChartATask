using System;

namespace ChartATask.Core.Requests.Evaluators
{
    public interface IEvaluator : IDisposable
    {
        bool Evaluate(IEvaluatorRequest request);
    }
}