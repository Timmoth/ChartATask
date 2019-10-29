using System;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;

namespace ChartATask.Core.Presenter
{
    public interface IPresenter : IDisposable
    {
        void Update(DataSet<DurationOverTime> dataSetCollection);
    }
}