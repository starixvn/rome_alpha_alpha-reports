namespace Alpha.Reports.Tests.Models
{
    using System;
    using Constants;
    using Reports.Models;
    using Xunit;

    public class BalanceUpDayAndMoveBetsDayTests
    {
        [Fact]
        public void BalanceUpDay_Always_ReturnsDateSetFromCtor()
        {
            // Arrange
            var balanceUpDay = new DateTime(2000, 1, 1);
            var date = new BalanceUpDayAndMoveBetsDay(balanceUpDay, DefaultVariables.CurrentDate);

            // Act
            var actual = date.BalanceUpDay;

            // Assert
            Assert.Equal(balanceUpDay, actual);
        }

        [Fact]
        public void MoveBetsDay_Always_ReturnsDateSetFromCtor()
        {
            // Arrange
            var moveBetsDay = new DateTime(2000, 1, 1);
            var date = new BalanceUpDayAndMoveBetsDay(DefaultVariables.CurrentDate, moveBetsDay);

            // Act
            var actual = date.MoveBetsDay;

            // Assert
            Assert.Equal(moveBetsDay, actual);
        }
    }
}
