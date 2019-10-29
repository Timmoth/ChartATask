using ChartATask.Core.Models.Acceptor;

namespace ChartATask.Core.Models.Events.AppEvents
{
    public class AppTitleEventSocket : IEventSocket<AppTitleEvent>
    {
        private readonly IAcceptor<string> _nameAcceptor;
        private readonly IAcceptor<bool> _shownAcceptor;
        private readonly IAcceptor<string> _titleAcceptor;

        public AppTitleEventSocket(IAcceptor<string> nameAcceptor, IAcceptor<string> titleAcceptor,
            IAcceptor<bool> shownAcceptor)
        {
            _nameAcceptor = nameAcceptor;
            _titleAcceptor = titleAcceptor;
            _shownAcceptor = shownAcceptor;
        }

        public bool Accepts(AppTitleEvent eventTrigger)
        {
            return _nameAcceptor.Accepts(eventTrigger.Name) &&
                   _titleAcceptor.Accepts(eventTrigger.Title) &&
                   _shownAcceptor.Accepts(eventTrigger.Shown);
        }
    }
}