using System;
using System.Collections.Generic;
using ChartATask.Models.Events;

namespace ChartATask.Models
{


    public interface IDataSource
    {
        List<Trigger> Triggers { get; }
        void Trigger(IEvent newEvent, IEvaluator evaluator);

    }   
    
    public interface IDataSource<T> : IDataSource where T : IDataPoint
    {

        event EventHandler<T> OnNewDataPoint;
    }
}