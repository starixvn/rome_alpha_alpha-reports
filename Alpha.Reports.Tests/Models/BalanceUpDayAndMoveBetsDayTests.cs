namespace Alpha.Reports.Tests.Models
{
    using System;
    using NSubstitute;
    using Reports.Models;
    using Xunit;

    public class BalanceUpDayAndMoveBetsDayTests
    {
        [Fact]
        public void BalanceUpDay_Always_GetDateSetFromCtor()
        {
            // Arrange
            var balanceUpDay = new DateTime(2000, 1, 1);
            var date = new BalanceUpDayAndMoveBetsDay(balanceUpDay, Arg.Any<DateTime>());

            // Act
            var actual = date.BalanceUpDay;

            // Assert
            Assert.Equal(balanceUpDay, actual);
        }

        [Fact]
        public void MoveBetsDay_Always_GetDateSetFromCtor()
        {
            // Arrange
            var moveBetsDay = new DateTime(2000, 1, 1);
            var date = new BalanceUpDayAndMoveBetsDay(Arg.Any<DateTime>(), moveBetsDay);

            // Act
            var actual = date.MoveBetsDay;

            // Assert
            Assert.Equal(moveBetsDay, actual);
        }
    }
}
