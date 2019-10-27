using System;
using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Interactors;
using ChartATask.Core.Models;
using ChartATask.Core.Models.Events;

namespace ChartATask.Core
{
    internal class EventFilter : IDisposable
    {
        private readonly Dictionary<IEvent, List<IDataSource>> _dataSources;

        public EventFilter(DataSetCollection dataSetCollection)
        {
            _dataSources = new Dictionary<IEvent, List<IDataSource>>();
            LoadActions(dataSetCollection.DataSources);
        }

        public List<IEvent> GetEvents()
        {
            return _dataSources.Select(entry => entry.Key).ToList();
        }

        public void Apply(Queue<IEvent> events, ISystemEvaluator evaluator)
        {
            var partiallyTriggeredActions = new HashSet<IDataSource>();

            foreach (var triggeredEvent in events)
            {
                if (!_dataSources.TryGetValue(triggeredEvent, out var triggeredDataSources))
                {
                    continue;
                }

                foreach (var triggeredAction in triggeredDataSources.Where(triggeredAction => !partiallyTriggeredActions.Contains(triggeredAction)))
                {
                    partiallyTriggeredActions.Add(triggeredAction);
                    triggeredAction.Trigger(triggeredEvent, evaluator);
                }
            }
        }

        private void LoadActions(IEnumerable<IDataSource> dataSources)
        {
            foreach (var dataSource in dataSources)
            {
                foreach (var triggerEvent in dataSource.Triggers.SelectMany(trigger => trigger.Events))
                {
                    if (!_dataSources.TryGetValue(triggerEvent, out var dataSourceList))
                    {
                        dataSourceList = new List<IDataSource>();
                        _dataSources.Add(triggerEvent, dataSourceList);
                    }

                    dataSourceList.Add(dataSource);
                }
            }
        }

        public void Dispose()
        {
            _dataSources.Clear();
        }
    }
}