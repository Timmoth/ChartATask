using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly DataSetCollection _dataSetCollection;
        private readonly EventFilter _eventFilter;
        private readonly IPresenter _presenter;
        private readonly ISystemInteractor _systemInteractor;

        private CancellationTokenSource _cancellationTokenSource;

        private bool _isRunning;

        public Engine(IPresenter presenter, ISystemInteractor systemSystemInteractor)
        {
            _presenter = presenter;
            _systemInteractor = systemSystemInteractor;

            _dataSetCollection = new DataSetCollection(new List<IDataSet>
            {
                new KeyPressDataSet()
            });

            _eventFilter = new EventFilter(_dataSetCollection);
            _systemInteractor.EventWatcher.SetListeners(_eventFilter.GetEvents());
        }

        public void Dispose()
        {
            _systemInteractor?.Dispose();
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
            _systemInteractor.EventWatcher.Stop();
        }

        private async Task Run()
        {
            _isRunning = true;

            await new TaskFactory(_cancellationTokenSource.Token).StartNew(async () =>
            {
                _systemInteractor.EventWatcher.Start();

                while (_isRunning)
                {
                    var events = _systemInteractor.EventWatcher.GetEvents();
                    _eventFilter.Apply(events, _systemInteractor.SystemEvaluator);
                    _presenter.Update(_dataSetCollection);

                    await Task.Delay(100).ConfigureAwait(false);
                }
            }, _cancellationTokenSource.Token).ConfigureAwait(false);
        }
    }
}