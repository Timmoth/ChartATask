using System;
using System.Collections.Generic;
using System.Text;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Models
{
    public class AppSessionDataSet : IDataSet
    {
        private readonly List<DurationOverTime> _dataPoints;
        private readonly AppSessionDataSource _dataSource;
        public readonly string AppName;
        public readonly string AppTitle;

        public AppSessionDataSet(string appName, string appTitle)
        {
            AppName = appName;
            AppTitle = appTitle;
            _dataSource = new AppSessionDataSource(AppName, AppTitle);
            _dataPoints = new List<DurationOverTime>();

            _dataSource.OnNewDataPoint += DataSource_OnNewDataPoint;
        }

        public IDataSource DataSource => _dataSource;
        public IEnumerable<IDataPoint> DataPoints => _dataPoints;

        public void Add(DurationOverTime dataPoint)
        {
            _dataPoints.Add(dataPoint);
        }

        private void DataSource_OnNewDataPoint(object sender, DurationOverTime e)
        {
            _dataPoints.Add(e);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var totalSessionDuration = TimeSpan.Zero;
            foreach (var dataPoint in _dataPoints)
            {
                totalSessionDuration += dataPoint.Y;
            }

            builder.AppendLine(
                $"Duration: {totalSessionDuration}");

            return builder.ToString();
        }
    }
}