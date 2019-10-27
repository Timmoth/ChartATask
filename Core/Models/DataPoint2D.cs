namespace ChartATask.Core.Models
{
    public class DataPoint2D<x, y> : IDataPoint
    {
        public x X { get; }
        public y Y { get; }

        protected DataPoint2D(x X, y Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public override bool Equals(object obj)
        {
            return obj is DataPoint2D<x, y> dataPoint && dataPoint.X.Equals(X);
        }

        protected bool Equals(DataPoint2D<x, y> other)
        {
            return other != null && Equals(X, other.X) && Equals(Y, other.Y);
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }
}