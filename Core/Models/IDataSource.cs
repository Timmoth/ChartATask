using System;
using ChartATask.Core.Events;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public interface IDataSource<TDataPoint> where TDataPoint : IDataPoint
    {
        event EventHandler<TDataPoint> OnNewDataPoint;
        void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator);
    }
}