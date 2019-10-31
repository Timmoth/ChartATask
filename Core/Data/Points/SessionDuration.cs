using System;

namespace ChartATask.Core.Data.Points
{
    public class SessionDuration : DataPoint2D<DateTime, TimeSpan>
    {
        public SessionDuration(DateTime x, TimeSpan y) : base(x, y)
        {
        }

        public override string ToString()
        {
            return $@"{X.ToString()},{Y.ToString()}";
        }
    }
}