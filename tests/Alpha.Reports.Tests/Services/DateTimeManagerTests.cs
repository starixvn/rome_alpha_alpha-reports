namespace Alpha.Reports.Tests.Services
{
    using Constants;
    using Reports.Services;
    using Utils;
    using Xunit;

    public class DateTimeManagerTests
    {
        [Fact]
        public void Now_Always_ReturnsValueSetToNowProperty()
        {
            // Arrange
            var expectedDate = DefaultVariables.CurrentDate;
            SystemTime.Today = expectedDate;

            // Act
            var actualDate = SystemTime.Today;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}