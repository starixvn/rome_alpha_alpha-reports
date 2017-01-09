namespace Alpha.Reports.Tests.Extensions
{
    using System;
    using Reports.Extensions;
    using Xunit;

    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(12, 0, 0)]
        [InlineData(13, 0, 0)]
        [InlineData(23, 59, 59)]
        public void GetBeginningOfDate_Always_ReturnsBeginningOfDate(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedTime = new DateTime(2016, 11, 24, 0, 0, 0);

            // Act
            var actualTime = dateTime.GetBeginningOfDate();

            // Assert
            Assert.Equal(expectedTime, actualTime);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 0, 0)]
        [InlineData(12, 0, 0)]
        [InlineData(13, 0, 0)]
        [InlineData(23, 59, 59)]
        public void ToShortDayWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedDay = "11/24/2016";

            // Act
            var actualDay = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }

        [Theory]
        [InlineData(0, 0, 0, "11/24/2016 12:00:00 AM")]
        [InlineData(1, 0, 0, "11/24/2016 01:00:00 AM")]
        [InlineData(11, 59, 59, "11/24/2016 11:59:59 AM")]
        [InlineData(12, 0, 0, "11/24/2016 12:00:00 PM")]
        [InlineData(13, 0, 0, "11/24/2016 01:00:00 PM")]
        [InlineData(23, 59, 59, "11/24/2016 11:59:59 PM")]
        public void ToLongDateWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second, string expectedDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);

            // Act
            var actualDay = dateTime.ToLongDateWithSlash();

            // Assert
            Assert.Equal(expectedDay, actualDay);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 1)]
        [InlineData(31, 1)]
        [InlineData(1, 6)]
        [InlineData(2, 6)]
        [InlineData(30, 6)]
        [InlineData(1, 12)]
        [InlineData(2, 12)]
        [InlineData(31, 12)]
        public void FirstDayOfMonth_Always_ReturnsFirstDayOfMonth(int day, int month)
        {
            // Arrange
            var dateTime = new DateTime(2016, month, day);

            // Act
            var actualDate = dateTime.FirstDayOfMonth();

            // Assert
            Assert.Equal(month, actualDate.Month);
            Assert.Equal(1, actualDate.Day);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 15)]
        [InlineData(1, 31)]
        [InlineData(6, 1)]
        [InlineData(6, 15)]
        [InlineData(6, 30)]
        [InlineData(12, 1)]
        [InlineData(12, 15)]
        [InlineData(12, 31)]
        public void FirstDayOfYear_Always_ReturnsFirstDayOfYear(int month, int day)
        {
            // Arrange
            var date = new DateTime(2016, month, day);
            var expectedDate = new DateTime(2016, 1, 1);

            // Act
            var actualDate = date.FirstDayOfYear();

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }
    }
}
