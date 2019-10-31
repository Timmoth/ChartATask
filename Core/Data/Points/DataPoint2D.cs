namespace ChartATask.Core.DataPoints
{
    public class DataPoint2D<x, y> : IDataPoint
    {
        protected DataPoint2D(x X, y Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public x X { get; }
        public y Y { get; }

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