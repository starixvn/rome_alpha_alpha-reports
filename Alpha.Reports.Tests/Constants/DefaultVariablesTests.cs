namespace Alpha.Reports.Tests.Constants
{
    using System;
    using NUnit.Framework;
    using Reports.Constants;

    [TestFixture]
    public class DefaultVariablesTests
    {
        [Test]
        public void GetCacheItemOptions_Always_SlidingExpirationIsOneHour()
        {
            // Arrange
            var expectedTimeSpan = TimeSpan.FromHours(1);

            // Act
            var cacheItemOptions = DefaultVariables.CacheItemOptions;

            // Assert
            Assert.AreEqual(cacheItemOptions.SlidingExpiration, expectedTimeSpan);
        }
    }
}
