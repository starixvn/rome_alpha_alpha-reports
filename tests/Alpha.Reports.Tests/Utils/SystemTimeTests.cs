namespace Alpha.Reports.Tests.Utils
{
    using Constants;
    using Reports.Utils;
    using Xunit;

    public class SystemTimeTests
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