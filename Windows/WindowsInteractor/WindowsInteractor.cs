using System.Collections.Concurrent;
using System.Collections.Generic;
using ChartATask.Models;

namespace ChartATask.Interactors.Windows
{
    public class WindowsInteractor : IInteractor
    {
        private readonly ConcurrentQueue<IInteractionEvent> _eventQueue;

        public WindowsInteractor()
        {
            _eventQueue = new ConcurrentQueue<IInteractionEvent>();
        }


        public Queue<IInteractionEvent> GetEvents()
        {
            var newEvents = new Queue<IInteractionEvent>();
            while (_eventQueue.TryDequeue(out var newEvent))
            {
                newEvents.Enqueue(newEvent);
            }

            return newEvents;
        }

        public void SetListeners(List<CoreAction> actionManagerActions)
        {
        }
    }
}