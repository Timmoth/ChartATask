using System;
using ChartATask.Core.DataPoints;
using ChartATask.Core.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public interface IDataSource<TDataPoint> where TDataPoint : IDataPoint
    {
        event EventHandler<TDataPoint> OnNewDataPoint;
        void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator);
    }
}