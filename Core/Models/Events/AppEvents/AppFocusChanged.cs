namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppFocusChanged : IEvent
    {
        public readonly string Name;
        public readonly string Title;

        public AppFocusChanged(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public override string ToString()
        {
            return $"AppFocusChanged: \n\tName: {Name}\n\tTitle: {Title}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppFocusChanged other && other.Name == Name && other.Title == Title;
        }

        public override int GetHashCode()
        {
            return (Name, Title).GetHashCode();
        }
    }
}