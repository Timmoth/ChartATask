namespace ChartATask.Core.Events
{
    public interface IEventSocket
    {
        bool Accepts(IEvent eventTrigger);
    }
}