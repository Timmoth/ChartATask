using ChartATask.Core.Triggers.Events;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CoreTests
{
    public class EventWatcherTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static List<Mock<EventWatcher>> GetMockEventWatchers(int count)
        {
            var mockEventWatcherList = new List<Mock<EventWatcher>>();
            for (var i = 0; i < count; i++)
            {
                var mockEventWatcher = new Mock<EventWatcher>(MockBehavior.Loose);
                mockEventWatcherList.Add(mockEventWatcher);
            }

            return mockEventWatcherList;
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void StartWatchers(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);
            var _eventWatcherManager = new EventWatcherManager(mockEventWatchers.Select(o => o.Object));

            _eventWatcherManager.Start();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Start(), Times.Once));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void StopsWatchers(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);

            var _eventWatcherManager = new EventWatcherManager(mockEventWatchers.Select(o => o.Object));


            _eventWatcherManager.Stop();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Stop(), Times.Once));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void DisposesWatchers(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);

            var _eventWatcherManager = new EventWatcherManager(mockEventWatchers.Select(o => o.Object));


            _eventWatcherManager.Dispose();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Dispose(), Times.Once));
            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Stop(), Times.Once));
        }
    }
}