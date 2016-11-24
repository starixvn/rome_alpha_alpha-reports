namespace Alpha.Reports.Tests.Extensions
{
    using System;
    using NUnit.Framework;
    using Reports.Extensions;

    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(12, 0, 0)]
        [TestCase(13, 0, 0)]
        [TestCase(23, 59, 59)]
        public void GetBeginningOfDate_Always_ReturnsBeginningOfDate(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedTime = new DateTime(2016, 11, 24, 0, 0, 0);

            // Act
            var actualTime = dateTime.GetBeginningOfDate();

            // Assert
            Assert.AreEqual(expectedTime, actualTime);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(12, 0, 0)]
        [TestCase(13, 0, 0)]
        [TestCase(23, 59, 59)]
        public void GetEndingOfDate_Always_ReturnsEndingOfDate(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedTime = new DateTime(2016, 11, 24, 23, 59, 59);

            // Act
            var actualTime = dateTime.GetEndingOfDate();

            // Assert
            Assert.AreEqual(expectedTime, actualTime);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 0)]
        [TestCase(12, 0, 0)]
        [TestCase(13, 0, 0)]
        [TestCase(23, 59, 59)]
        public void ToShortDayWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);
            var expectedDay = "11/24/2016";

            // Act
            var actualDay = dateTime.ToShortDateWithSlash();

            // Assert
            Assert.AreEqual(expectedDay, actualDay);
        }

        [TestCase(0, 0, 0, "11/24/2016 12:00:00 AM")]
        [TestCase(1, 0, 0, "11/24/2016 01:00:00 AM")]
        [TestCase(11, 59, 59, "11/24/2016 11:59:59 AM")]
        [TestCase(12, 0, 0, "11/24/2016 12:00:00 PM")]
        [TestCase(13, 0, 0, "11/24/2016 01:00:00 PM")]
        [TestCase(23, 59, 59, "11/24/2016 11:59:59 PM")]
        public void ToLongDateWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second, string expectedDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);

            // Act
            var actualDay = dateTime.ToLongDateWithSlash();

            // Assert
            Assert.AreEqual(expectedDay, actualDay);
        }

        [TestCase(0, 0, 0, "11/24/2016<br/>12:00:00 AM")]
        [TestCase(1, 0, 0, "11/24/2016<br/>01:00:00 AM")]
        [TestCase(11, 59, 59, "11/24/2016<br/>11:59:59 AM")]
        [TestCase(12, 0, 0, "11/24/2016<br/>12:00:00 PM")]
        [TestCase(13, 0, 0, "11/24/2016<br/>01:00:00 PM")]
        [TestCase(23, 59, 59, "11/24/2016<br/>11:59:59 PM")]
        public void ToHtmlLongDateWithSlash_Always_ReturnsShortDayWithSlash(int hour, int minute, int second, string expectedDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, hour, minute, second);

            // Act
            var actualDay = dateTime.ToHtmlLongDateWithSlash();

            // Assert
            Assert.AreEqual(expectedDay, actualDay);
        }

        [TestCase(0, 0, 0)]
        [TestCase(1, 0, 1)]
        [TestCase(-4, 0, 20)]
        public void ToOffsetHour_BeginningOfDate_ReturnsCorrectTime(int gmt, int hour, int expectedHour)
        {
            // Arrange
            var dateTime = new DateTime(2016, 11, 24, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var actualHour = dateTime.ToOffsetHour(gmt).Hour;

            // Assert
            Assert.AreEqual(expectedHour, actualHour);
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(31, 1)]
        [TestCase(1, 6)]
        [TestCase(2, 6)]
        [TestCase(30, 6)]
        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(31, 12)]
        public void FirstDayOfMonth_Always_ReturnsFirstDayOfMonth(int day, int month)
        {
            // Arrange
            var dateTime = new DateTime(2016, month, day);

            // Act
            var actualDate = dateTime.FirstDayOfMonth();

            // Assert
            Assert.AreEqual(month, actualDate.Month);
            Assert.AreEqual(1, actualDate.Day);
        }

        [TestCase(1, 1, 31)]
        [TestCase(2, 1, 31)]
        [TestCase(31, 1, 31)]
        [TestCase(1, 6, 30)]
        [TestCase(2, 6, 30)]
        [TestCase(30, 6, 30)]
        [TestCase(1, 12, 31)]
        [TestCase(2, 12, 31)]
        [TestCase(31, 12, 31)]
        public void LastDayOfMonth_Always_ReturnsLastDayOfMonth(int day, int month, int expectedLastDay)
        {
            // Arrange
            var dateTime = new DateTime(2016, month, day);

            // Act
            var actualDate = dateTime.LastDayOfMonth();

            // Assert
            Assert.AreEqual(month, actualDate.Month);
            Assert.AreEqual(expectedLastDay, actualDate.Day);
        }
    }
}
