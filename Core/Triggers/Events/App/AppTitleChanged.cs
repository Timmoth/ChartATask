namespace ChartATask.Core.Triggers.Events.App
{
    public class AppTitleChanged : IEvent
    {
        public string Name { get; }
        public string Title { get; }

        public AppTitleChanged(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public override string ToString()
        {
            return $"AppTitleChanged: \n\tName: {Name}\n\tTitle: {Title}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppTitleChanged other &&
                string.Compare(other.Name, Name) == 0 &&
                string.Compare(other.Title, Title) == 0;
        }

        public override int GetHashCode()
        {
            return (Name, Title).GetHashCode();
        }
    }
}