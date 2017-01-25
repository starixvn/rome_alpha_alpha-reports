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
            DateTimeUtility.Today = expectedDate;

            // Act
            var actualDate = DateTimeUtility.Today;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}