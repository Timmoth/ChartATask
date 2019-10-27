using System;

namespace ChartATask.Core.Models.DataPoints
{
    public class DurationOverTime : DataPoint2D<DateTime, TimeSpan>
    {
        public DurationOverTime(DateTime x, TimeSpan y) : base(x, y)
        {
        }

        public override string ToString()
        {
            return $@"{{{X.ToString()},{Y.ToString()}}}";
        }
    }
}