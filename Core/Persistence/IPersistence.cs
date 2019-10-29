using System;
using ChartATask.Core.Models;

namespace ChartATask.Core.Persistence
{
    public interface IPersistence : IDisposable
    {
        void Save(DataSetCollection dataSetCollection);
        DataSetCollection Load();
    }
}