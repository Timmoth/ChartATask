namespace ChartATask.Interactors.Windows.EventWatcher
{
    public class WindowsEventWatcher : Core.Interactors.EventWatcher
    {
        public WindowsEventWatcher() : base(
            new WindowsKeyboardWatcher(),
            new WindowsAppWatcher())
        {

        }
    }
}