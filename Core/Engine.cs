using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Events;
using ChartATask.Core.Models;
using ChartATask.Core.Presenter;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly DataSetCollection _dataSetCollection;
        private readonly EventCollector _eventCollector;
        private readonly EventFilter _eventFilter;
        private readonly IPresenter _presenter;
        private readonly RequestEvaluator _requestEvaluator;
        private bool _isRunning;

        public Engine(
            IPresenter presenter,
            EventCollector systemEventCollector,
            RequestEvaluator requestEvaluator,
            DataSetCollection dataSetCollection)
        {
            _presenter = presenter;
            _eventCollector = systemEventCollector;
            _requestEvaluator = requestEvaluator;
            _dataSetCollection = dataSetCollection;
            _eventFilter = new EventFilter(_dataSetCollection);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _eventFilter?.Dispose();
            _eventCollector?.Dispose();
            _requestEvaluator?.Dispose();
            _cancellationTokenSource.Dispose();
        }

        public void Start()
        {
            Run().ConfigureAwait(false);
        }

        public void Stop()
        {
            _isRunning = false;
            _cancellationTokenSource.CancelAfter(100);
        }

        private async Task Run()
        {
            if (_eventCollector == null || _requestEvaluator == null)
            {
                throw new NullReferenceException();
            }

            _eventCollector.Start();
            _requestEvaluator.Start();

            await new TaskFactory(_cancellationTokenSource.Token).StartNew(async () =>
            {
                _isRunning = true;

                while (_isRunning)
                {
                    var events = _eventCollector.GetEvents();
                    _eventFilter.Apply(events, _requestEvaluator);

                    if (events.Any())
                    {
                        _presenter.Update(_dataSetCollection);
                    }

                    await Task.Delay(100).ConfigureAwait(false);
                }
            }, _cancellationTokenSource.Token).ContinueWith(o =>
            {
                _eventCollector.Stop();
                _requestEvaluator.Stop();
            }).ConfigureAwait(false);
        }
    }
}