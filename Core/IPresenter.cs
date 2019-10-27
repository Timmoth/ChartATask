using System;
using ChartATask.Core.Models;

namespace ChartATask.Core
{
    public interface IPresenter : IDisposable
    {
        void Update(DataSetCollection dataSetCollection);
    }
}