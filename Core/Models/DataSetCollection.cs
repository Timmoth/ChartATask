using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Models
{
    public class DataSetCollection
    {
        private readonly List<IDataSet> _dataSets;

        public DataSetCollection(List<IDataSet> dataSets)
        {
            _dataSets = dataSets;
        }

        public IEnumerable<IDataSet> DataSets => _dataSets;
        public IEnumerable<IDataSource> DataSources => _dataSets.Select(dataSet => dataSet.DataSource);
    }
}