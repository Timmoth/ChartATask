using System;
using System.Collections.Generic;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core.Models
{
    public interface IDataSource
    {
        List<Trigger> Triggers { get; }
        void Trigger(IEvent newEvent, ISystemEvaluator evaluator);

    }   
    
    public interface IDataSource<T> : IDataSource where T : IDataPoint
    {

        event EventHandler<T> OnNewDataPoint;
    }
}