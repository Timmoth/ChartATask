namespace ChartATask.Core.Models.Events
{
    public class AppCloseEvent : IEvent
    {
        public readonly string Name;

        public AppCloseEvent(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $@"AppCloseEvent: {Name}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppCloseEvent appClosed && appClosed.Name == Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}