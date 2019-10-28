using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Models.Events.KeyboardEvents;

namespace ChartATask.Core.Models
{
    public class KeyPressDataSet : IDataSet
    {
        private readonly List<IntOverTime> _dataPoints;
        private readonly KeyPressedDataSource _dataSource;

        public KeyPressDataSet()
        {
            _dataSource = new KeyPressedDataSource();
            _dataPoints = new List<IntOverTime>();

            _dataSource.OnNewDataPoint += DataSource_OnNewDataPoint;
        }

        public IDataSource DataSource => _dataSource;
        public IEnumerable<IDataPoint> DataPoints => _dataPoints;

        private void DataSource_OnNewDataPoint(object sender, IntOverTime e)
        {
            _dataPoints.Add(e);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var trigger in _dataSource.Triggers)
            {
                builder.AppendLine(
                    $"DataSet\n{trigger}\nCount: {_dataPoints.Count(point => trigger.Events.Contains(new KeyPressedEvent(point.Y)))}");
            }

            return builder.ToString();
        }
    }
}