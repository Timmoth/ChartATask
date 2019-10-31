namespace ChartATask.Core.Triggers.Events
{
    public interface IEventSocket
    {
        bool Accepts(IEvent eventTrigger);
    }
}