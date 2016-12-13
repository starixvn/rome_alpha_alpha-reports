namespace Alpha.Reports.Tests.Comparers
{
    using Reports.Models.WinlossDetails;
    using Xunit;

    public class NetWinLossComparerTests
    {
        [Fact]
        public void Equals_OnlyNetWinLossAIsNull_ReturnFalse()
        {
            // Arrange
            NetWinLoss netWinLossA = null;

            var netWinLossB = new NetWinLoss(0, 0);

            // Act
            var actualEqual = NetWinLossComparer.Equals(netWinLossA, netWinLossB);

            // Assert
            Assert.False(actualEqual);
        }

        [Fact]
        public void Equals_OnlyNetWinLossBIsNull_ReturnFalse()
        {
            // Arrange
            var netWinLossA = new NetWinLoss(0, 0);

            NetWinLoss netWinLossB = null;

            // Act
            var actualEqual = NetWinLossComparer.Equals(netWinLossA, netWinLossB);

            // Assert
            Assert.False(actualEqual);
        }
    }
}
