using ChartATask.Core.Triggers.Events;

namespace ChartATask.Core.Triggers
{
    public interface ITrigger
    {
        bool IsTriggered(IEvent firedEvent);
    }
}