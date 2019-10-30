using System;

namespace ChartATask.Core.Requests
{
    public interface IRequestEvaluator : IDisposable
    {
        void Start();
        void Stop();
    }

    public interface IRequestEvaluator<in TRequest, out TValue> : IRequestEvaluator where TRequest : IRequest<TValue>
    {
        TValue Evaluate(TRequest request);
    }
}