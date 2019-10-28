namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppOpenEvent : IEvent
    {
        public readonly string Name;

        public AppOpenEvent(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"AppOpenEvent: \n\tName: {Name}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppOpenEvent appOpened && appOpened.Name == Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}