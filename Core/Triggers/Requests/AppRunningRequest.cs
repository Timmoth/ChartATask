namespace ChartATask.Core.Triggers.Requests
{
    public class AppRunningRequest : IRequest<bool>
    {
        public readonly string Name;

        public AppRunningRequest(string name)
        {
            Name = name;
        }
    }
}