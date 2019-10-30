using System;

namespace ChartATask.Core.Requests
{
    public class SystemTimeRequestEvaluator : IRequestEvaluator<SystemTimeRequest, DateTime>
    {
        public void Dispose()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public DateTime Evaluate(SystemTimeRequest request)
        {
            return DateTime.Now;
        }

        public string RequestTypeName()
        {
            return "SystemTimeRequest";
        }
    }
}