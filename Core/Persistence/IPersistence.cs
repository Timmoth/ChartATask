using ChartATask.Core.Data;
using System;
using System.Collections.Generic;

namespace ChartATask.Core.Persistence
{
    public interface IPersistence : IDisposable
    {
        void Save(IEnumerable<IDataSet> dataSet);
        IEnumerable<IDataSet> Load();
    }
}