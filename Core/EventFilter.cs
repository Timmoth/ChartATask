using System;
using System.Collections.Generic;
using ChartATask.Interactors;

namespace ChartATask.Core
{
    internal class EventFilter
    {
        private readonly ActionManager _actionManager;

        public EventFilter(ActionManager actionManager)
        {
            _actionManager = actionManager;
        }

        public List<Action> Filter(Queue<IInteractionEvent> events)
        {
            var triggeredActions = new List<Action>();
            //Find actions triggered by the given events
            //Check that any triggered actions have all passing events
            return triggeredActions;
        }
    }
}