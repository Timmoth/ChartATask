using System.Collections.Generic;

namespace ChartATask.Core.Models
{
    public interface IDataSet
    {
        IDataSource DataSource { get; }
        IEnumerable<IDataPoint> DataPoints { get; }
    }
}