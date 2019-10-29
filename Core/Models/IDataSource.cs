using System;
using System.Collections.Generic;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Models.Events;
using ChartATask.Core.Requests;

namespace ChartATask.Core.Models
{
    public interface IDataSource
    {
        List<Trigger> Triggers { get; }
        void Trigger(IEvent newEvent, RequestEvaluator evaluator);
    }

    public interface IDataSource<T> : IDataSource where T : IDataPoint
    {
        event EventHandler<T> OnNewDataPoint;
    }
}