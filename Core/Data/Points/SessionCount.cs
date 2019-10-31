using System;

namespace ChartATask.Core.Data.Points
{
    public class SessionCount : DataPoint2D<DateTime, int>
    {
        public SessionCount(DateTime x, int y) : base(x, y)
        {
        }

        public override string ToString()
        {
            return $@"{{{X.ToString()},{Y.ToString()}}}";
        }
    }
}