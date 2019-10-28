namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppCloseEvent : IEvent
    {
        public readonly string Name;
        public readonly int ProcessID;
        public readonly string Title;

        public AppCloseEvent(string name, int processId)
        {
            Name = name;
            ProcessID = processId;
        }

        public AppCloseEvent(string name) : this(name, 0)
        {
        }

        public override string ToString()
        {
            return $"AppCloseEvent: \n\tName: {Name}\n\tTitle: {Title}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppCloseEvent appOpened && appOpened.Name == Name && appOpened.Title == Title;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}