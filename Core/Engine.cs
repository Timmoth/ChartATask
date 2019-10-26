using System;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Interactors;
using ChartATask.Presenters;

namespace ChartATask.Core
{
    public class Engine : IDisposable
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

        public void Dispose()
        {
            _interactor?.Dispose();
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Run().ConfigureAwait(false);
        }

        public void Stop()
        {
            _isRunning = false;
            _cancellationTokenSource.CancelAfter(100);
            _interactor.Stop();
        }

        private async Task Run()
        {
            _isRunning = true;

            await new TaskFactory(_cancellationTokenSource.Token).StartNew(() =>
            {
                _interactor.Start();

                while (_isRunning)
                {
                    var events = _interactor.GetEvents();
                    var triggeredActions = _eventFilter.Filter(events);
                    foreach (var triggeredAction in triggeredActions)
                    {
                        Console.WriteLine(triggeredAction.ToString());
                    }

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