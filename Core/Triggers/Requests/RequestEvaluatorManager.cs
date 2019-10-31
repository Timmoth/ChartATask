using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Triggers.Requests
{
    public class RequestEvaluatorManager
    {
        private readonly List<IRequestEvaluator> _requestEvaluators;

        public RequestEvaluatorManager(IEnumerable<IRequestEvaluator> requestEvaluators)
        {
            _requestEvaluators = new List<IRequestEvaluator>(requestEvaluators) {new SystemTimeRequestEvaluator()};
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