using System.Collections.Generic;
using ChartATask.Models;

namespace ChartATask.Interactors
{
    public interface IInteractionEvent
    {
    }

    public interface IInteractor
    {
        Queue<IInteractionEvent> GetEvents();
        void SetListeners(List<CoreAction> actionManagerActions);
    }
}