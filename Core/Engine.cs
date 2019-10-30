using System;
using System.Collections.Generic;
using ChartATask.Core.Events;
using ChartATask.Core.Models;
using ChartATask.Core.Persistence;
using ChartATask.Core.Presenter;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly List<IDataSet> _dataSets;
        private readonly EventWatcherManager _eventWatcherManager;
        private readonly IPersistence _persistence;
        private readonly IPresenter _presenter;
        private readonly RequestEvaluator _requestEvaluator;

        public Engine(
            IPersistence persistence,
            IPresenter presenter,
            EventWatcherManager eventWatcherManager,
            RequestEvaluator requestEvaluator)
        {
            _presenter = presenter;
            _eventWatcherManager = eventWatcherManager;
            _requestEvaluator = requestEvaluator;
            _dataSets = new List<IDataSet>();
            _persistence = persistence;
        }

        public void Dispose()
        {
            _eventWatcherManager?.Dispose();
            _requestEvaluator?.Dispose();
        }

        public void Start()
        {
            _eventWatcherManager.Start();
            _requestEvaluator.Start();
        }

        public void Stop()
        {
            _eventWatcherManager.Stop();
            _requestEvaluator.Stop();
        }

        public void Load(string directory)
        {
            _dataSets.AddRange(_persistence.Load(directory));
            foreach (var dataSet in _dataSets)
            {
                dataSet.Setup(_eventWatcherManager, _requestEvaluator);
            }
        }

        public void Save()
        {
            _persistence.Save(_dataSets);
        }

        public void Show()
        {
            _presenter.Update(_dataSets);
        }
    }
}