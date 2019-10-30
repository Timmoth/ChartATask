using System;

namespace ChartATask.Core.Requests
{
    public class SystemTimeRequest : IRequest<DateTime>
    {
        public override string ToString()
        {
            return "SystemTimeRequest";
        }
    }
}