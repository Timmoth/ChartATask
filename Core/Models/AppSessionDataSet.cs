using System.Collections.Generic;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Models
{
    public class DataSet<TDataPoint> where TDataPoint : IDataPoint
    {
        private readonly List<TDataPoint> _dataPoints;

        public DataSet()
        {
            _dataPoints = new List<TDataPoint>();
        }

        public IDataSource<TDataPoint> DataSource { get; private set; }

        public IEnumerable<TDataPoint> DataPoints => _dataPoints;

        public void Setup(IDataSource<TDataPoint> dataSource)
        {
            DataSource = dataSource;
            DataSource.OnNewDataPoint += DataSource_OnNewDataPoint;
        }

        public void Add(TDataPoint dataPoint)
        {
            _dataPoints.Add(dataPoint);
        }

        private void DataSource_OnNewDataPoint(object sender, TDataPoint e)
        {
            _dataPoints.Add(e);
        }

        public override string ToString()
        {
            return string.Join("\n", _dataPoints);
        }
    }
}