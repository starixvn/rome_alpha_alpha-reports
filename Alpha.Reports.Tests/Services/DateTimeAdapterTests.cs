namespace Alpha.Reports.Tests.Services
{
    using System;
    using Reports.Services;
    using Xunit;

    public class DateTimeAdapterTests
    {
        [Fact]
        public void Now_Always_ReturnsDateNotLaterThan1Millisecond()
        {
            // Act
            var actualDate = DateTimeAdapter.Now;

            // Assert
            Assert.True((DateTime.Now - actualDate).Milliseconds <= 1);
        }
    }
}
