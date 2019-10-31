using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Triggers.Requests
{
    public class RequestEvaluatorManager
    {
        private readonly List<IRequestEvaluator> _requestEvaluators;

        public RequestEvaluatorManager()
        {
            _requestEvaluators = new List<IRequestEvaluator>
            {
                new SystemTimeRequestEvaluator()
            };
        }

        public void Register(IRequestEvaluator eventEventWatcher)
        {
            _requestEvaluators.Add(eventEventWatcher);
        }

        public T GetRequestEvaluator<T>() where T : IRequestEvaluator
        {
            return _requestEvaluators.OfType<T>().FirstOrDefault();
        }

        public void Start()
        {
            foreach (var eventWatcher in _requestEvaluators)
            {
                eventWatcher.Start();
            }
        }

        public void Stop()
        {
            foreach (var eventWatcher in _requestEvaluators)
            {
                eventWatcher.Stop();
            }
        }

        public void Dispose()
        {
            Stop();
            foreach (var eventWatcher in _requestEvaluators)
            {
                eventWatcher.Dispose();
            }
        }
    }
}