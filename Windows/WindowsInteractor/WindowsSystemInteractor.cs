using ChartATask.Core.Interactors;

namespace ChartATask.Interactors.Windows
{
    public class WindowsSystemInteractor : ISystemInteractor
    {
        public IEventWatcher EventWatcher { get; }
        public ISystemEvaluator SystemEvaluator { get; }

        public WindowsSystemInteractor()
        {
            EventWatcher = new WindowsEventWatcher();
            SystemEvaluator = new WindowsSystemEvaluator();
        }

        public void Dispose()
        {
            EventWatcher?.Dispose();
            SystemEvaluator?.Dispose();
        }
    }
}
