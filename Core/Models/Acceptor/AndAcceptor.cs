using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Models.Acceptor
{
    public class AndAcceptor<TValue> : IAcceptor<TValue>
    {
        private readonly IEnumerable<IAcceptor<TValue>> _acceptors;

        public AndAcceptor(IEnumerable<IAcceptor<TValue>> acceptors)
        {
            _acceptors = acceptors;
        }

        public bool Accepts(TValue input)
        {
            return _acceptors.All(p => p.Accepts(input));
        }
    }
}