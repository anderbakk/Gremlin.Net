﻿using System;
using System.Collections.Generic;
using System.Linq;
using Gremlin.Net.Process.Traversal;
using Moq;
using Xunit;

namespace Gremlin.Net.Process.UnitTest.Traversal
{
    public class TraversalTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData("test")]
        public void ShouldReturnAvailableTraverserObjWhenNextIsCalled(object traverserObj)
        {
            var traversal = new TestTraversal(new List<object> {traverserObj});

            var actualObj = traversal.Next();

            Assert.Equal(traverserObj, actualObj);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(10)]
        public void ShouldReturnCorrectNrOfResultObjsWhenNextIsCalledWithAmountArgument(int nrOfResults)
        {
            var objs = new List<object>(20);
            for (var i = 0; i < 20; i++)
                objs.Add(i);
            var traversal = new TestTraversal(objs);

            var traversedObjs = traversal.Next(nrOfResults);

            var traversedObjsList = traversedObjs.ToList();
            Assert.Equal(nrOfResults, traversedObjsList.Count);
            for (var i = 0; i < nrOfResults; i++)
                Assert.Equal(objs[i], traversedObjsList[i]);
        }

        private List<object> UnfoldBulks(IReadOnlyList<object> objs, IReadOnlyList<long> bulks)
        {
            var unfoldedObjs = new List<object>();
            for (var traverserIdx = 0; traverserIdx < objs.Count; traverserIdx++)
            for (var currentBulkObjIdx = 0; currentBulkObjIdx < bulks[traverserIdx]; currentBulkObjIdx++)
                unfoldedObjs.Add(objs[traverserIdx]);
            return unfoldedObjs;
        }

        [Fact]
        public void ShouldDrainAllTraversersWhenIterateIsCalled()
        {
            var someObjs = new List<object> {1, 2, 3};
            var traversal = new TestTraversal(someObjs);

            var drainedTraversal = traversal.Iterate();

            Assert.Null(drainedTraversal.Next());
        }

        [Fact]
        public void ShouldReturnNullWhenNextIsCalledAndNoTraverserIsAvailable()
        {
            var expectedFirstObj = 1;
            var traversal = new TestTraversal(new List<object> {expectedFirstObj});

            var actualFirstObj = traversal.Next();
            var actualSecondObj = traversal.Next();

            Assert.Equal(expectedFirstObj, actualFirstObj);
            Assert.Null(actualSecondObj);
        }

        [Fact]
        public void ShouldReturnTraversalsTraverserWhenNextTraverserIsCalled()
        {
            var someObjs = new List<object> {1, 2, 3};
            var traversal = new TestTraversal(someObjs);

            var traverser = traversal.NextTraverser();

            Assert.Equal(traversal.Traversers.First(), traverser);
        }

        [Fact]
        public void ShouldThrowNotSupportedExceptionWhenResetIsCalled()
        {
            var someObjs = new List<object> {1, 2, 3};
            var traversal = new TestTraversal(someObjs);

            Assert.Throws<NotSupportedException>(() => traversal.Reset());
        }

        [Fact]
        public void ShouldReturnAllTraverserObjsWhenToListIsCalled()
        {
            var expectedObjs = new List<object> {1, 2, 3};
            var traversal = new TestTraversal(expectedObjs);

            var traversedObjs = traversal.ToList();

            Assert.Equal(expectedObjs, traversedObjs);
        }

        [Fact]
        public void ShouldApplyStrategiesWhenNextIsCalledAndNoTraversersPresent()
        {
            const int expectedObj = 531;
            var testStrategy = new TestTraversalStrategy(new List<Traverser> {new Traverser(expectedObj)});
            var testTraversal = new TestTraversal(new List<ITraversalStrategy> {testStrategy});

            var actualObj = testTraversal.Next();

            Assert.Equal(expectedObj, actualObj);
        }

        [Fact]
        public void ShouldBeUnfoldTraverserBulksWhenToListIsCalled()
        {
            var objs = new List<object> {1, 2, 3};
            var bulks = new List<long> {3, 2, 1};
            var traversal = new TestTraversal(objs, bulks);

            var traversedObjs = traversal.ToList();

            var expectedObjs = UnfoldBulks(objs, bulks);
            Assert.Equal(expectedObjs, traversedObjs);
        }

        [Fact]
        public void ShouldDisposeSideEffectsWhenDisposeIsCalled()
        {
            var sideEffectsMock = new Mock<ITraversalSideEffects>();
            var traversal = new TestTraversal(new List<object>()) {SideEffects = sideEffectsMock.Object};

            traversal.Dispose();

            sideEffectsMock.Verify(m => m.Dispose());
        }
    }
}