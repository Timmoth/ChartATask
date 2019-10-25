using System;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Interactors;
using ChartATask.Presenters;

namespace ChartATask.Core
{
    public class Engine
    {
        private readonly ActionManager _actionManager;
        private readonly ConditionFilter _conditionFilter;
        private readonly EventFilter _eventFilter;
        private readonly IInteractor _interactor;
        private readonly IPresenter _presenter;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isRunning;

        public Engine(IPresenter presenter, IInteractor interactor)
        {
            _presenter = presenter;
            _interactor = interactor;

            _actionManager = new ActionManager();
            _eventFilter = new EventFilter(_actionManager);
            _conditionFilter = new ConditionFilter();

            _interactor.SetListeners(_actionManager.Actions);
        }

        public async Task Run(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _isRunning = true;

            await new TaskFactory(_cancellationTokenSource.Token).StartNew(() =>
            {
                while (_isRunning)
                {
                    var events = _interactor.GetEvents();
                    Console.WriteLine("Getting triggered actions");
                    var triggeredActions = _eventFilter.Filter(events);
                    Console.WriteLine("Checking if triggered actions pass their conditions");
                    var acceptedActions = _conditionFilter.Filter(triggeredActions);

                    //Execute accepted Actions
                    //Actions can either:
                    //add an entry to a chart
                    //Or
                    //Modify the system
                    _presenter.Update();
                }
            }, _cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}