using ChartATask.Core.Data.Points;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;
using System;

namespace ChartATask.Core.Data.Sources
{
    public interface IDataSource<TDataPoint> where TDataPoint : IDataPoint
    {
        event EventHandler<TDataPoint> OnNewDataPoint;
        void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator);
    }
}