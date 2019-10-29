using ChartATask.Core.Models.Events.AppEvents;

namespace ChartATask.Core.Events.Watchers
{
    public interface IRunningAppWatcher : IWatcher<AppRunEvent>
    {
    }
}