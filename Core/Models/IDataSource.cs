using System;
using ChartATask.Core.Events;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public interface IDataSource
    {
        void Setup(EventWatchers eventWatchers, RequestEvaluator requestEvaluator);
    }

    public interface IDataSource<T> : IDataSource where T : IDataPoint
    {
        event EventHandler<T> OnNewDataPoint;
    }
}