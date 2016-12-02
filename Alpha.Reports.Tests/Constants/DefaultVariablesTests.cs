namespace Alpha.Reports.Tests.Constants
{
    using System;
    using Xunit;
    using ReportsConstants = Reports.Constants;

    public class DefaultVariablesTests
    {
        [Fact]
        public void GetCacheItemOptions_Always_SlidingExpirationIsOneHour()
        {
            // Arrange
            var expectedTimeSpan = TimeSpan.FromHours(1);

            // Act
            var cacheItemOptions = ReportsConstants.DefaultVariables.CacheItemOptions;

            // Assert
            Assert.Equal(cacheItemOptions.SlidingExpiration, expectedTimeSpan);
        }
    }
}
