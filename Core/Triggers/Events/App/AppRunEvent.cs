namespace ChartATask.Core.Triggers.Events.App
{
    public class AppRunEvent : IEvent
    {
        public string Name { get; }
        public bool Running { get; }

        public AppRunEvent(string name, bool running)
        {
            Name = name;
            Running = running;
        }

        public override string ToString()
        {
            return $"AppRunEvent: \n\tName: {Name}\n\tRunning{Running}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppRunEvent other && string.Compare(other.Name, Name) == 0 && other.Running == Running;
        }

        public override int GetHashCode()
        {
            return (Name, Running).GetHashCode();
        }
    }
}