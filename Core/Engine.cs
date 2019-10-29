using System;
using System.Threading;
using System.Threading.Tasks;
using ChartATask.Core.Events;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Presenter;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly DataSet<DurationOverTime> _dataSet;
        private readonly EventWatchers _eventWatchers;
        private readonly IPresenter _presenter;
        private readonly RequestEvaluator _requestEvaluator;
        private bool _isRunning;

        public Engine(
            IPresenter presenter,
            EventWatchers systemEventWatchers,
            RequestEvaluator requestEvaluator,
            DataSet<DurationOverTime> dataSet)
        {
            _presenter = presenter;
            _eventWatchers = systemEventWatchers;
            _requestEvaluator = requestEvaluator;
            _dataSet = dataSet;
            var source = new AppSessionDataSource();
            source.Setup(systemEventWatchers, requestEvaluator);
            _dataSet.Setup(source);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _eventWatchers?.Dispose();
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
            if (_eventWatchers == null || _requestEvaluator == null)
            {
                throw new NullReferenceException();
            }

            _eventWatchers.Start();
            _requestEvaluator.Start();

            await new TaskFactory(_cancellationTokenSource.Token).StartNew(async () =>
            {
                _isRunning = true;

                while (_isRunning)
                {
                    _presenter.Update(_dataSet);

                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }, _cancellationTokenSource.Token).ContinueWith(o =>
            {
                _eventWatchers.Stop();
                _requestEvaluator.Stop();
            }).ConfigureAwait(false);
        }
    }
}