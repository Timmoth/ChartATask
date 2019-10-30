using ChartATask.Core.Events.App;
using ChartATask.Core.Models.Acceptor;

namespace ChartATask.Core.Events.Sockets
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