namespace ChartATask.Core.Triggers.Requests
{
    public class AppTitleRequest : IRequest<string>
    {
        public readonly string Name;

        public AppTitleRequest(string name)
        {
            Name = name;
        }
    }
}