namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppTitleEvent : IEvent
    {
        public readonly string Name;
        public readonly bool Shown;
        public readonly string Title;

        public AppTitleEvent(string name, string title, bool shown)
        {
            Name = name;
            Title = title;
            Shown = shown;
        }

        public override string ToString()
        {
            return $"AppTitleEvent: \n\tName: {Name}\n\tTitle: {Title}\n\tShown: {Shown}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppTitleEvent other && other.Name == Name && other.Title == Title && other.Shown == Shown;
        }

        public override int GetHashCode()
        {
            return (Name, Title, Shown).GetHashCode();
        }
    }
}