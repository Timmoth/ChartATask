namespace ChartATask.Core.Triggers.Events.App
{
    public class AppFocusChanged : IEvent
    {
        public string Name { get; }
        public string Title { get; }

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
            return obj is AppFocusChanged other &&
                string.Compare(other.Name, Name) == 0 &&
                string.Compare(other.Title, Title) == 0;
        }

        public override int GetHashCode()
        {
            return (Name, Title).GetHashCode();
        }
    }
}