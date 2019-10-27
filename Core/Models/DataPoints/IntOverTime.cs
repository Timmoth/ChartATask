using System;

namespace ChartATask.Core.Models
{
    public class IntOverTime : DataPoint2D<DateTime, int>
    {
        public IntOverTime(DateTime x, int y) : base(x, y)
        {

        }

        public override string ToString()
        {
            return $@"{{{X.ToString()},{Y.ToString()}}}";
        }
    }
}