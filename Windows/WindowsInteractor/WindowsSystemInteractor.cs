using ChartATask.Core.Interactors;
using ChartATask.Interactors.Windows.EventWatcher;
using ChartATask.Interactors.Windows.SystemEvaluator;

namespace ChartATask.Interactors.Windows
{
    public class WindowsSystemInteractor : ISystemInteractor
    {
        public WindowsSystemInteractor()
        {
            EventWatcher = new WindowsEventWatcher();
            SystemEvaluator = new WindowsSystemEvaluator();
        }

        public IEventWatcher EventWatcher { get; }
        public ISystemEvaluator SystemEvaluator { get; }

        public void Dispose()
        {
            EventWatcher?.Dispose();
            SystemEvaluator?.Dispose();
        }
    }
}