using ChartATask.Core.Models.Events.KeyboardEvents;

namespace ChartATask.Core.Events.Watchers
{
    public interface IKeyboardWatcher : IWatcher<KeyPressedEvent>
    {
    }
}