using System.Collections.Generic;

namespace ChartATask.Models
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
    }
}