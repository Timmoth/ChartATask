using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models
{
    public class CoreAction
    {
        public readonly List<ICondition> Conditions;
        public readonly List<IInteractionEvent> Triggers;

        public CoreAction(IEnumerable<IInteractionEvent> triggers, IEnumerable<ICondition> conditions)
        {
            Triggers = new List<IInteractionEvent>(triggers);
            Conditions = new List<ICondition>(conditions);
        }

        public override string ToString()
        {
            return $@"Action Triggers{{{string.Join(", ", Triggers)}}}";
        }

        public override bool Equals(object obj)
        {
            return obj is CoreAction coreAction && coreAction.Triggers.SequenceEqual(Triggers);
        }

        protected bool Equals(CoreAction other)
        {
            return Equals(Triggers, other.Triggers);
        }

        public override int GetHashCode()
        {
            return Triggers != null ? Triggers.GetHashCode() : 0;
        }
    }
}