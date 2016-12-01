namespace Alpha.Reports.Tests.Constants
{
    using System;
    using Reports.Constants;
    using Xunit;

    public class DefaultVariablesTests
    {
        [Fact]
        public void GetCacheItemOptions_Always_SlidingExpirationIsOneHour()
        {
            // Arrange
            var expectedTimeSpan = TimeSpan.FromHours(1);

            // Act
            var cacheItemOptions = DefaultVariables.CacheItemOptions;

            // Assert
            Assert.Equal(cacheItemOptions.SlidingExpiration, expectedTimeSpan);
        }
    }
}
