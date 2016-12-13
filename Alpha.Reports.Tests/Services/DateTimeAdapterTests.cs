namespace Alpha.Reports.Tests.Services
{
    using Constants;
    using Reports.Services;
    using Xunit;

    public class DateTimeAdapterTests
    {
        public DateTimeAdapterTests()
        {
            DateTimeAdapter.Now = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void Now_Always_ReturnsCurrentDate()
        {
            // Arrange
            var expectedDate = DefaultVariables.CurrentDate;

            // Act
            var actualDate = DateTimeAdapter.Now;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}
