namespace Alpha.Reports.Tests.Models
{
    using System;
    using Alpha.Reports.Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpDayAndMoveBetsDayTests
    {
        [Test]
        public void BalanceUpDay_Always_GetDateSetFromCtor()
        {
            // Arrange
            var balanceUpDay = new DateTime(2000, 1, 1);            
            var date = new BalanceUpDayAndMoveBetsDay(balanceUpDay, Arg.Any<DateTime>());

            // Act
            var actual = date.BalanceUpDay;

            // Assert
            Assert.AreEqual(balanceUpDay, actual);
        }

        [Test]
        public void MoveBetsDay_Always_GetDateSetFromCtor()
        {
            // Arrange
            var moveBetsDay = new DateTime(2000, 1, 1);
            var date = new BalanceUpDayAndMoveBetsDay(Arg.Any<DateTime>(), moveBetsDay);

            // Act
            var actual = date.MoveBetsDay;

            // Assert
            Assert.AreEqual(moveBetsDay, actual);
        }
    }
}
