﻿using Gremlin.Net.Process.Traversal.Strategy;
using Gremlin.Net.Process.Traversal.Strategy.Optimization;
using Gremlin.Net.Process.Traversal.Strategy.Verification;
using Xunit;

namespace Gremlin.Net.Process.UnitTest.Traversal.Strategy
{
    public class StrategyTests
    {
        [Fact]
        public void ShouldReturnFalseForEqualsOfStrategiesWithDifferentStrategyNames()
        {
            var firstStrategy = new TestStrategy("aConfigKey", "aConfigValue");
            var secondStrategy = new IncidentToAdjacentStrategy();

            var areEqual = firstStrategy.Equals(secondStrategy);

            Assert.False(areEqual);
        }

        [Fact]
        public void ShouldReturnTrueForEqualsOfStrategiesWithEqualNamesButDifferentConfigurations()
        {
            var firstStrategy = new TestStrategy("aConfigKey", "aConfigValue");
            var secondStrategy = new TestStrategy("anotherKey", "anotherValue");

            var areEqual = firstStrategy.Equals(secondStrategy);

            Assert.True(areEqual);
        }

        [Fact]
        public void ShouldReturnDifferentHashcodesForStrategiesWithDifferentNames()
        {
            var firstStrategy = new TestStrategy();
            var secondStrategy = new ReadOnlyStrategy();

            var firstHashCode = firstStrategy.GetHashCode();
            var secondHashCode = secondStrategy.GetHashCode();

            Assert.NotEqual(firstHashCode, secondHashCode);
        }

        [Fact]
        public void ShouldReturnEqualHashcodesForStrategiesWithEqualNamesButDifferentConfigurations()
        {
            var firstStrategy = new TestStrategy("aConfigKey", "aConfigValue");
            var secondStrategy = new TestStrategy("anotherKey", "anotherValue");

            var firstHashCode = firstStrategy.GetHashCode();
            var secondHashCode = secondStrategy.GetHashCode();

            Assert.Equal(firstHashCode, secondHashCode);
        }

        [Fact]
        public void ShouldReturnClassNameForStrategyNameProperty()
        {
            var testStrategy = new TestStrategy();

            Assert.Equal("TestStrategy", testStrategy.StrategyName);
        }

        [Fact]
        public void ShouldReturnStrategyNameWhenForToString()
        {
            var testStrategy = new TestStrategy();

            var strategyStr = testStrategy.ToString();

            Assert.Equal("TestStrategy", strategyStr);
        }
    }

    internal class TestStrategy : AbstractTraversalStrategy
    {
        public TestStrategy()
        {
        }

        public TestStrategy(string configKey, dynamic configValue)
        {
            Configuration[configKey] = configValue;
        }
    }
}