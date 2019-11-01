using System;

namespace ChartATask.Core.Data.Points
{
    public class SessionDuration : DataPoint2D<DateTime, DateTime>
    {
        public SessionDuration(DateTime x, DateTime y) : base(x, y)
        {
        }

        public override string ToString()
        {
            return $@"{X.ToString()},{Y.ToString()}";
        }
    }
}