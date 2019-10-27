using System;

namespace ChartATask.Core.Models.Events
{
    public class AppSessionEvent : IEvent
    {
        public readonly string Name;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public TimeSpan Duration => EndTime - StartTime;

        public AppSessionEvent(string name, DateTime startTime, DateTime endTime)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString()
        {
            return $"AppSessionEvent: {Name}\n\tStart Time: {StartTime}\n\tEnd Time: {EndTime}\n\tDuration: {Duration}";
        }

        public override bool Equals(object obj)
        {
            return obj is AppSessionEvent appOpened && appOpened.Name == Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}