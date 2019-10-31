namespace ChartATask.Core.Triggers.Events.App
{
    public class AppTitleChanged : IEvent
    {
        public readonly string Name;
        public readonly string Title;

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
            return obj is AppTitleChanged other && other.Name == Name && other.Title == Title;
        }

        public override int GetHashCode()
        {
            return (Name, Title).GetHashCode();
        }
    }
}