using System;
using System.Collections.Generic;
using ChartATask.Core.Events;
using ChartATask.Core.Models;
using ChartATask.Core.Models.DataPoints;
using ChartATask.Core.Persistence;
using ChartATask.Core.Presenter;
using ChartATask.Core.Requests;

namespace ChartATask.Core
{
    public class Engine : IDisposable
    {
        private readonly List<DataSet<DurationOverTime>> _dataSets;
        private readonly IPersistence<DurationOverTime> _persistence;
        private readonly EventWatchers _eventWatchers;
        private readonly IPresenter _presenter;
        private readonly RequestEvaluator _requestEvaluator;

        public Engine(
            IPersistence<DurationOverTime> persistence,
            IPresenter presenter,
            EventWatchers eventWatchers,
            RequestEvaluator requestEvaluator)
        {
            _presenter = presenter;
            _eventWatchers = eventWatchers;
            _requestEvaluator = requestEvaluator;
            _dataSets = new List<DataSet<DurationOverTime>>();
            _persistence = persistence;
        }

        public void Start()
        {
            _eventWatchers.Start();
            _requestEvaluator.Start();
        }

        public void Stop()
        {
            _eventWatchers.Stop();
            _requestEvaluator.Stop();
        }

        public void Dispose()
        {
            _eventWatchers?.Dispose();
            _requestEvaluator?.Dispose();
        }

        public void Load(string fileName)
        {
            var dataSet = _persistence.Load(fileName);
            var source = new AppSessionDataSource();
            source.Setup(_eventWatchers, _requestEvaluator);
            dataSet.Setup(source);
            _dataSets.Add(dataSet);
        }

        public void Save()
        {
            foreach (var dataSet in _dataSets)
            {
                _persistence.Save(dataSet);
            }
        }

        public void Show()
        {
            foreach (var dataSet in _dataSets)
            {
                _presenter.Update(dataSet);
            }
        }
    }
}