using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Models
{
    public class KeyPressDataSet : IDataSet
    {
        private readonly KeyPressedDataSource _dataSource;
        private readonly List<IntOverTime> _dataPoints;
        public IDataSource DataSource => _dataSource;
        public IEnumerable<IDataPoint> DataPoints => _dataPoints;

        public KeyPressDataSet()
        {
            _dataSource = new KeyPressedDataSource();
            _dataPoints = new List<IntOverTime>();

            _dataSource.OnNewDataPoint += DataSource_OnNewDataPoint;  
        }

        private void DataSource_OnNewDataPoint(object sender, IntOverTime e)
        {
            _dataPoints.Add(e);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var trigger in _dataSource.Triggers)
            {
                builder.AppendLine($"DataSet\n{trigger}\nCount: { _dataPoints.Count(point => trigger.Events.Contains(new KeyPressedEvent(point.Y)))}");
            }

            return builder.ToString();
        }
    }
}