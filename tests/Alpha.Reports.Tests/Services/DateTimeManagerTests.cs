namespace Alpha.Reports.Tests.Services
{
    using Constants;
    using Reports.Services;
    using Xunit;

    public class DateTimeManagerTests
    {
        [Fact]
        public void Now_Always_ReturnsValueSetToNowProperty()
        {
            // Arrange
            var expectedDate = DefaultVariables.CurrentDate;
            DateTimeManager.Now = expectedDate;

            // Act
            var actualDate = DateTimeManager.Now;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}
