using ChartATask.Core.Models.Acceptor;

namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppTitleEventSocket : IEventSocket<AppTitleChangedEvent>
    {
        private readonly IAcceptor<string> _nameAcceptor;
        private readonly IAcceptor<string> _titleAcceptor;

        public AppTitleEventSocket(
            IAcceptor<string> nameAcceptor,
            IAcceptor<string> titleAcceptor)
        {
            _nameAcceptor = nameAcceptor;
            _titleAcceptor = titleAcceptor;
        }

        public bool Accepts(AppTitleChangedEvent changedEventTrigger)
        {
            return _nameAcceptor.Accepts(changedEventTrigger.Name) &&
                   _titleAcceptor.Accepts(changedEventTrigger.Title);
        }
    }
}