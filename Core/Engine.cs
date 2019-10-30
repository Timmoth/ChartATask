﻿using System;
using System.Collections.Generic;
using ChartATask.Core.Events;
using ChartATask.Core.Models;
using ChartATask.Core.Persistence;
using ChartATask.Core.Requests;

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
        }

        public void Dispose()
        {
            Stop();
            _eventWatcherManager?.Dispose();
            _requestEvaluatorManager?.Dispose();
            _persistence?.Dispose();
        }

        public void Start()
        {
            Load();
            _eventWatcherManager?.Start();
            _requestEvaluatorManager?.Start();
        }

        public void Stop()
        {
            _eventWatcherManager?.Stop();
            _requestEvaluatorManager?.Stop();
            _persistence.Save(_dataSets);
        }

        private void Load()
        {
            _dataSets.AddRange(_persistence.Load());
            _dataSets.ForEach(dataSet => dataSet.Setup(_eventWatcherManager, _requestEvaluatorManager));
        }
    }
}