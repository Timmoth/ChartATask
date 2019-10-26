using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Models
{
    public class CoreAction
    {
        public readonly List<IInteractionEvent> Triggers;

        public CoreAction(IEnumerable<IInteractionEvent> triggers)
        {
            Triggers = new List<IInteractionEvent>(triggers);
        }

        public override string ToString()
        {
            return $@"Action Triggers{{{string.Join(", ", Triggers)}}}";
        }

        public override bool Equals(object obj)
        {
            return obj is CoreAction coreAction && coreAction.Triggers.SequenceEqual(this.Triggers);
        }

        protected bool Equals(CoreAction other)
        {
            return Equals(Triggers, other.Triggers);
        }

        public override int GetHashCode()
        {
            return (Triggers != null ? Triggers.GetHashCode() : 0);
        }
    }
}