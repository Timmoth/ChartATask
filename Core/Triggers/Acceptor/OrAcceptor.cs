﻿using System.Collections.Generic;
using System.Linq;

namespace ChartATask.Core.Triggers.Acceptor
{
    public class OrAcceptor<TValue> : IAcceptor<TValue>
    {
        private readonly IEnumerable<IAcceptor<TValue>> _acceptors;

        public OrAcceptor(IEnumerable<IAcceptor<TValue>> acceptors)
        {
            _acceptors = acceptors;
        }

        public bool Accepts(TValue input)
        {
            return _acceptors.Any(p => p.Accepts(input));
        }
    }
}