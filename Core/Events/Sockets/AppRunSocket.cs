using ChartATask.Core.Events.App;
using ChartATask.Core.Models.Acceptor;

namespace ChartATask.Core.Events.Sockets
{
    public class AppRunSocket : IEventSocket
    {
        private readonly IAcceptor<string> _nameAcceptor;
        private readonly IAcceptor<bool> _runningAcceptor;

        public AppRunSocket(
            IAcceptor<string> nameAcceptor,
            IAcceptor<bool> runningAcceptor)
        {
            _nameAcceptor = nameAcceptor;
            _runningAcceptor = runningAcceptor;
        }

        public bool Accepts(IEvent eventTrigger)
        {
            return eventTrigger is AppRunEvent appRunEvent &&
                   _nameAcceptor.Accepts(appRunEvent.Name) &&
                   _runningAcceptor.Accepts(appRunEvent.Running);
        }

        public override string ToString()
        {
            return "AppRunSocket";
        }
    }
}