namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsEventCollector : Core.Interactors.EventCollector
    {
        public WindowsEventCollector() : base(
            new WindowsKeyboardWatcher(),
            new WindowsAppWatcher())
        {

        }
    }
}