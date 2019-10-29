using System;
using ChartATask.Core.Models;

namespace ChartATask.Core
{
    public interface IPersistence : IDisposable
    {
        void Save(DataSetCollection dataSetCollection);
        DataSetCollection Load();
    }
}