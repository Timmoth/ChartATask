using System;

namespace ChartATask.Core.Interactors
{
    public interface ISystemInteractor : IDisposable
    {
        IEventWatcher EventWatcher { get; }
        ISystemEvaluator SystemEvaluator { get; }
    }
}