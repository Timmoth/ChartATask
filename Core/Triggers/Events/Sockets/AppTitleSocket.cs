using ChartATask.Core.Triggers.Acceptor;
using ChartATask.Core.Triggers.Events.App;

namespace ChartATask.Core.Triggers.Events.Sockets
{
    public class AppTitleSocket : IEventSocket
    {
        private readonly IAcceptor<string> _nameAcceptor;
        private readonly IAcceptor<string> _titleAcceptor;

        public AppTitleSocket(
            IAcceptor<string> nameAcceptor,
            IAcceptor<string> titleAcceptor)
        {
            _nameAcceptor = nameAcceptor;
            _titleAcceptor = titleAcceptor;
        }

        public bool Accepts(IEvent eventTrigger)
        {
            return eventTrigger is AppTitleChanged appTitleChanged &&
                   _nameAcceptor.Accepts(appTitleChanged.Name) &&
                   _titleAcceptor.Accepts(appTitleChanged.Title);
        }

        public override string ToString()
        {
            return "AppTitleSocket";
        }
    }
}