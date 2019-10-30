using ChartATask.Core.Models.Acceptor;

namespace ChartATask.Core.Events.AppEvents
{
    public class AppFocusSocket : IEventSocket
    {
        private readonly IAcceptor<string> _nameAcceptor;
        private readonly IAcceptor<string> _titleAcceptor;

        public AppFocusSocket(
            IAcceptor<string> nameAcceptor,
            IAcceptor<string> titleAcceptor)
        {
            _nameAcceptor = nameAcceptor;
            _titleAcceptor = titleAcceptor;
        }

        public bool Accepts(IEvent eventTrigger)
        {
            return eventTrigger is AppFocusChanged appFocusChanged &&
                   _nameAcceptor.Accepts(appFocusChanged.Name) &&
                   _titleAcceptor.Accepts(appFocusChanged.Title);
        }

        public override string ToString()
        {
            return "AppFocusSocket";
        }
    }
}