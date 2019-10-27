using System.Collections.Generic;

namespace ChartATask.Models
{
    public interface IDataSet
    {
        IDataSource DataSource { get; }
        IEnumerable<IDataPoint> DataPoints { get; }
    }
}