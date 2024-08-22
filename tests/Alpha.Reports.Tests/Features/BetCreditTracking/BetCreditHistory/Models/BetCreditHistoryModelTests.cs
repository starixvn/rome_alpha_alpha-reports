namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory.Models
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using System;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class BetCreditHistoryModelTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var actual = new BetCreditHistoryModel();

            // Assert
            Assert.True(
                actual.RefNo == null &&
                actual.ActualStake == null &&
                actual.WinLoss == null &&
                actual.PlayerComm == null &&
                actual.BalanceChanged == null &&
                actual.NewBalance == null &&
                actual.GivenCredit == 0 &&
                actual.BetCredit == null &&
                actual.ActionId == 0 &&
                actual.Action == null &&
                actual.IsUserLogActionId == false);
        }

        [Fact]
        public void ActionDateString_HasData_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BetCreditHistoryModel(new DateTime(2022, 3, 3, 23, 59, 00), DateTime.Now, 123456, 10, 10, 10, 10, 10, 10, 10, 1, "Bet Updated", false);

            // Act
            var actual = entity.ActionDateString;

            // Assert
            Assert.Equal("03/03/2022 23:59:00", actual);
        }

        [Fact]
        public void WinLostDateString_Null_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BetCreditHistoryModel(DateTime.Now, null, 123456, 10, 10, 10, 10, 10, 10, 10, 1, "Bet Updated", false);

            // Act
            var actual = entity.WinLostDateString;

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void WinLostDateString_HasData_ReturnsCorrectString()
        {
            // Arrange
            var entity = new BetCreditHistoryModel(DateTime.Now, new DateTime(2022, 3, 3, 23, 59, 00), 123456, 10, 10, 10, 10, 10, 10, 10, 1, "Bet Updated", false);

            // Act
            var actual = entity.WinLostDateString;

            // Assert
            Assert.Equal("03/03/2022", actual);
        }
    }
}
