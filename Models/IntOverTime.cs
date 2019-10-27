using System;

namespace ChartATask.Models
{
    public class IntOverTime : DataPoint2D<DateTime, int>
    {
        public IntOverTime(DateTime x, int y) : base(x, y)
        {

        }
    }
}