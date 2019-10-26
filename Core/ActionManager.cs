using System.Collections.Generic;
using ChartATask.Models;

namespace ChartATask.Core
{
    internal class ActionManager
    {
        public readonly List<CoreAction> Actions;

        public ActionManager()
        {
            Actions = new List<CoreAction>
            {
                new CoreAction(new List<IInteractionEvent> {new KeyPressedEvent(65)}),
                new CoreAction(new List<IInteractionEvent> {new KeyPressedEvent(65)}),
                new CoreAction(new List<IInteractionEvent> {new KeyPressedEvent(66)})
            };
        }
    }
}