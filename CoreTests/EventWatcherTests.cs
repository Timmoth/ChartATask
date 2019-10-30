using System.Collections.Generic;
using System.Linq;
using ChartATask.Core.Events;
using ChartATask.Core.Models.Events;
using Moq;
using NUnit.Framework;

namespace CoreTests
{
    public class EventWatcherTests
    {
        private EventWatcherManager _eventWatcherManager;

        [SetUp]
        public void Setup()
        {
            _eventWatcherManager = new EventWatcherManager();
        }

        private static List<Mock<IEventWatcher<IEvent>>> GetMockEventWatchers(int count)
        {
            var mockEventWatcherList = new List<Mock<IEventWatcher<IEvent>>>();
            for (var i = 0; i < count; i++)
            {
                var mockEventWatcher = new Mock<IEventWatcher<IEvent>>(MockBehavior.Loose);
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

            mockEventWatchers.ForEach(m => _eventWatcherManager.Register(m.Object));

            _eventWatcherManager.Start();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Start(), Times.Once));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void StopsWatchers(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);

            mockEventWatchers.ForEach(m => _eventWatcherManager.Register(m.Object));

            _eventWatcherManager.Stop();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Stop(), Times.Once));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void DisposesWatchers(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);

            mockEventWatchers.ForEach(m => _eventWatcherManager.Register(m.Object));

            _eventWatcherManager.Dispose();

            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Dispose(), Times.Once));
            mockEventWatchers.ForEach(m => m.Verify(obj => obj.Stop(), Times.Once));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetWatcher(int count)
        {
            var mockEventWatchers = GetMockEventWatchers(count);
            mockEventWatchers.ForEach(m => _eventWatcherManager.Register(m.Object));
            var watchers = _eventWatcherManager.GetWatcher<IEvent>();

            CollectionAssert.AreEqual(mockEventWatchers.Select(m => m.Object), watchers);
        }
    }
}