﻿using System;
using System.Collections.Generic;
using ChartATask.Models;

namespace ChartATask.Interactors
{
    public interface IInteractor : IDisposable
    {
        Queue<IEvent> GetEvents();
        void SetListeners(List<IEvent> eventList);
        void Start();
        void Stop();
    }
}