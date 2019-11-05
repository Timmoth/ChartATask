using System;

namespace ChartATask.Core.Data.Points
{
    public class SessionDuration : DataPoint2D<DateTime, DateTime>
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public SessionDuration(string name, string title, DateTime x, DateTime y) : base(x, y)
        {
            Name = name;
            Title = title;
        }

        public override string ToString()
        {
            return $@"{Name},{Title},{X.ToString()},{Y.ToString()}";
        }
    }
}