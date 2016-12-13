namespace Alpha.Reports.Tests.Models
{
    using System;
    using Reports.Models;
    using Xunit;

    public class ReportApplicationContextTests
    {
        [Fact]
        public void CurrentDate_Always_ReturnsDateNotLaterThan1Millisecond()
        {
            // Arrange
            var reportApplicationContext = new ReportApplicationContext();

            // Act
            var actualDate = reportApplicationContext.CurrentDate;

            // Assert
            Assert.True((DateTime.Now - actualDate).Milliseconds <= 1);
        }
    }
}
