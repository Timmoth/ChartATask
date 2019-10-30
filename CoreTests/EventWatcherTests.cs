using ChartATask.Core.Events;
using NUnit.Framework;

namespace CoreTests
{
    public class EventWatcherTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartsWatchers()
        {
            EventWatcherManager eventWatcherManager = new EventWatcherManager();

        }

        [Test]
        public void StopsWatchers()
        {
            EventWatcherManager eventWatcherManager = new EventWatcherManager();

        }       
        
        [Test]
        public void DisposesWatchers()
        {
            EventWatcherManager eventWatcherManager = new EventWatcherManager();

        }
    }
}