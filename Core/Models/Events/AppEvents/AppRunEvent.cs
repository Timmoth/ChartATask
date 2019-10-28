namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppRunEvent : IEvent
    {
        public readonly string Name;
        public readonly bool Running;

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
            return obj is AppRunEvent other && other.Name == Name && other.Running == Running;
        }

        public override int GetHashCode()
        {
            return (Name, Running).GetHashCode();
        }
    }
}