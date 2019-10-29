namespace ChartATask.Core.Models.Events
{
    public interface IEventSocket<in TEvent> where TEvent : IEvent
    {
        bool Accepts(TEvent eventTrigger);
    }
}