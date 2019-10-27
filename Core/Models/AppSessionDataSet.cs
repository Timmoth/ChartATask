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

        public AppSessionDataSet()
        {
            _dataSource = new AppSessionDataSource("firefox");
            _dataPoints = new List<DurationOverTime>();

            _dataSource.OnNewDataPoint += DataSource_OnNewDataPoint;
        }

        public IDataSource DataSource => _dataSource;
        public IEnumerable<IDataPoint> DataPoints => _dataPoints;

        private void DataSource_OnNewDataPoint(object sender, DurationOverTime e)
        {
            _dataPoints.Add(e);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            TimeSpan totalSessionDuration = TimeSpan.Zero;
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