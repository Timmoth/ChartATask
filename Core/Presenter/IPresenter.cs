using System;
using System.Collections.Generic;
using ChartATask.Core.Models;

namespace ChartATask.Core.Presenter
{
    public interface IPresenter : IDisposable
    {
        void Update(List<IDataSet> dataSetCollection);
    }
}