using System.Collections.Generic;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Models
{
    public interface IDataSet
    {
        IDataSource DataSource { get; }
        IEnumerable<IDataPoint> DataPoints { get; }
    }
}