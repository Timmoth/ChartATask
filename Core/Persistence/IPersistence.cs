using System;
using System.Collections.Generic;
using ChartATask.Core.Models;

namespace ChartATask.Core.Persistence
{
    public interface IPersistence : IDisposable
    {
        void Save(IEnumerable<IDataSet> dataSet);
        IEnumerable<IDataSet> Load();
    }
}