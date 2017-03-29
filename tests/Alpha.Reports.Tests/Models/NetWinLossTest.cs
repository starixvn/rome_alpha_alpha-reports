namespace Alpha.Reports.Tests.Models
{
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class NetWinLossTest
    {
        private readonly CompareLogic compareLogic;

        public NetWinLossTest()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void NetWinLoss_WinLossComTotalParams_ReturnsModelFromCtor()
        {
            // Arrange
            var expectedWinlossModel = new NetWinLoss(5, (decimal)0.2, (decimal)5.2);

            // Act
            var actualModel = new NetWinLoss(
                expectedWinlossModel.WinLoss,
                expectedWinlossModel.Comm,
                expectedWinlossModel.Total);

            // Assert
            var result = compareLogic.Compare(actualModel, expectedWinlossModel);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_NullNetWinLoss_ReturnCurrentNetWinLoss()
        {
            // Arrange
            var netWinLossA = new NetWinLoss(5, (decimal)0.2, (decimal)5.2);
            NetWinLoss netWinLossB = null;

            // Act
            var actualNetWinLoss = NetWinLoss.Add(netWinLossA, netWinLossB);

            // Assert
            var result = compareLogic.Compare(netWinLossA, actualNetWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidNetWinLoss_ReturnCorrectData()
        {
            // Arrange
            var netWinLossA = new NetWinLoss(5, (decimal)0.2, (decimal)5.2);
            var netWinLossB = new NetWinLoss(5, (decimal)0.2, (decimal)5.2);
            var expectedNetWinLoss = new NetWinLoss(10, (decimal)0.4, (decimal)10.4);

            // Act
            var actualNetWinLoss = NetWinLoss.Add(netWinLossA, netWinLossB);

            // Assert
            var result = compareLogic.Compare(expectedNetWinLoss, actualNetWinLoss);
            Assert.True(result.AreEqual);
        }
    }
}