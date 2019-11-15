using ChartATask.Core.Data;
using ChartATask.Core.Persistence;
using ChartATask.Core.Triggers.Events;
using ChartATask.Core.Triggers.Requests;
using System;
using System.Collections.Generic;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly List<IDataSet> _dataSets;
        private readonly EventWatcherManager _eventWatcherManager;
        private readonly IPersistence _persistence;
        private readonly RequestEvaluatorManager _requestEvaluatorManager;

        public Engine(
            IPersistence persistence,
            EventWatcherManager eventWatcherManager,
            RequestEvaluatorManager requestEvaluatorManager)
        {
            _dataSets = new List<IDataSet>();

            _persistence = persistence;
            _eventWatcherManager = eventWatcherManager;
            _requestEvaluatorManager = requestEvaluatorManager;


            Load();
            _eventWatcherManager?.Start();
            _requestEvaluatorManager?.Start();
        }

        public void Dispose()
        {
            _eventWatcherManager?.Stop();
            _requestEvaluatorManager?.Stop();

            _persistence.Save(_dataSets);

            _eventWatcherManager?.Dispose();
            _requestEvaluatorManager?.Dispose();
            _persistence?.Dispose();
        }

        private void Load()
        {
            _dataSets.AddRange(_persistence.Load());
            _dataSets.ForEach(dataSet => dataSet.Setup(_eventWatcherManager, _requestEvaluatorManager));
        }

        public IEnumerable<IDataSet> GetDataSets()
        {
            return _dataSets;
        }
    }
}