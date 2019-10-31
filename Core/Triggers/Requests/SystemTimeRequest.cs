using System;

namespace ChartATask.Core.Triggers.Requests
{
    public class SystemTimeRequest : IRequest<DateTime>
    {
        public override string ToString()
        {
            return "SystemTimeRequest";
        }
    }
}