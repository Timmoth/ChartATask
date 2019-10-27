using System;

namespace ChartATask.Core.Interactors
{
    public interface ISystemInteractor : IDisposable
    {
        EventWatcher EventWatcher { get; }
        ISystemEvaluator SystemEvaluator { get; }
    }
}