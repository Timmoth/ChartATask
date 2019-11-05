using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;

namespace ChartATask.Core.Data
{
    public interface IDataSet
    {
        void Setup(EventWatcherManager eventWatcherManager, RequestEvaluatorManager requestEvaluator);
    }
}