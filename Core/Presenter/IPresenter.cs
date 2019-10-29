using System;
using ChartATask.Core.Models;

namespace ChartATask.Core.Presenter
{
    public interface IPresenter : IDisposable
    {
        void Update(DataSetCollection dataSetCollection);
    }
}