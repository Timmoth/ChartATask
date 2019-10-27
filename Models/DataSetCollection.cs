using System;
using System.Collections.Generic;
using System.Text;

namespace ChartATask.Models
{
    public class DataSetCollection
    {
        private readonly List<IDataSet> _dataSets;

        public IEnumerable<IDataSet> DataSets => _dataSets;
        public IEnumerable<IDataSource> DataSources => _dataSets.Select(dataSet => dataSet.DataSource);

        public DataSetCollection(List<IDataSet> dataSets)
        {
            _dataSets = dataSets;
        }

    }
}
