﻿using ChartATask.Core.Triggers.Acceptor;
using ChartATask.Core.Triggers.Requests;
using System;

namespace ChartATask.Core.Triggers.Conditions
{
    public class SystemTimeCondition : ICondition
    {
        private readonly IAcceptor<DateTime> _acceptor;
        private readonly SystemTimeRequest _request;
        private SystemTimeRequestEvaluator _requestEvaluator;

        public SystemTimeCondition(IAcceptor<DateTime> acceptor)
        {
            _acceptor = acceptor;
            _request = new SystemTimeRequest();
        }

        public void Setup(RequestEvaluatorManager requestEvaluatorManager)
        {
            _requestEvaluator =
                requestEvaluatorManager.GetRequestEvaluator<SystemTimeRequestEvaluator>();
        }

        public bool Check()
        {
            return _acceptor.Accepts(_requestEvaluator.Evaluate(_request));
        }
    }
}