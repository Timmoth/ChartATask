using System;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Persistence
{
    public interface IPersistence<TDataPoint> : IDisposable where TDataPoint : IDataPoint
    {
        void Save(DataSet<TDataPoint> dataSet);
        DataSet<TDataPoint> Load(string fileName);
    }
}