using System.Collections.Generic;
using ChartATask.Core.DataPoints;
using ChartATask.Core.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public interface IDataSet
    {
        void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator);
    }

    public class DataSet<TDataPoint> : IDataSet where TDataPoint : IDataPoint
    {
        private readonly List<TDataPoint> _dataPoints;
        private readonly IDataSource<TDataPoint> _dataSource;

        public DataSet(IDataSource<TDataPoint> dataSource)
        {
            _dataSource = dataSource;
            _dataSource.OnNewDataPoint += DataSource_OnNewDataPoint;
            _dataPoints = new List<TDataPoint>();
        }

        public IEnumerable<TDataPoint> DataPoints => _dataPoints;

        public void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator)
        {
            _dataSource.Setup(eventWatcherManager, requestEvaluator);
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