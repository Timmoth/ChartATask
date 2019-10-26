using System.Collections.Generic;
using System.Linq;
using ChartATask.Models;

namespace ChartATask.Core
{
    internal class EventFilter
    {
        private readonly ActionManager _actionManager;
        public Dictionary<IInteractionEvent, List<CoreAction>> Actions;

        public EventFilter(ActionManager actionManager)
        {
            _actionManager = actionManager;

            Actions = new Dictionary<IInteractionEvent, List<CoreAction>>();
            LoadActions();
        }

        public List<CoreAction> Filter(Queue<IInteractionEvent> events)
        {
            var partiallyTriggeredActions = new HashSet<CoreAction>();

            foreach (var triggeredEvent in events)
            {
                if (!Actions.TryGetValue(triggeredEvent, out var actions))
                {
                    continue;
                }

                foreach (var triggeredAction in actions)
                {
                    partiallyTriggeredActions.Add(triggeredAction);
                }
            }

            var fullyTriggeredActions = partiallyTriggeredActions
                .Where(action => action.Triggers.All(events.Contains)).ToList();

            return fullyTriggeredActions;
        }

        private void LoadActions()
        {
            foreach (var action in _actionManager.Actions)
            {
                foreach (var eventTrigger in action.Triggers)
                {
                    if (!Actions.TryGetValue(eventTrigger, out var actions))
                    {
                        actions = new List<CoreAction>();
                        Actions.Add(eventTrigger, actions);
                    }

                    actions.Add(action);
                }
            }
        }
    }
}