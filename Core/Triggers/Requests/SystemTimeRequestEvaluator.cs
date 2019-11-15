using System;

namespace ChartATask.Core.Triggers.Requests
{
    public class SystemTimeRequestEvaluator : IRequestEvaluator<SystemTimeRequest, DateTime>
    {
        public void Dispose()
        {
            //No need to dispose
        }

        public void Start()
        {
            //No Start needed, Always running
        }

        public void Stop()
        {
            //No Stop needed, Always running
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