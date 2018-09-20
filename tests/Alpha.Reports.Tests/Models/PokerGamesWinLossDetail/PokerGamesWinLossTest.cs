namespace Alpha.Reports.Tests.Models
{
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models.PokerGamesWinLossDetail;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    public class PokerGamesWinLossTest
    {
        private readonly CompareLogic compareLogic;

        public PokerGamesWinLossTest()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void PokerGamesWinLoss_Default_ReturnsModelFromCtor()
        {
            // Act
            var actualModel = new PokerGamesWinLoss();

            // Assert
            Assert.True(actualModel.WinLoss == null && actualModel.InsuranceGemsWinLoss == null && actualModel.Comm == null && actualModel.Total == 0);
        }

        [Fact]
        public void PokerGamesWinLoss_WinLossComTotalParams_ReturnsModelFromCtor()
        {
            // Arrange
            var expectedWinlossModel = new PokerGamesWinLoss(5, 0, (decimal)0.2, (decimal)5.2);

            // Act
            var actualModel = new PokerGamesWinLoss(
                expectedWinlossModel.WinLoss,
                expectedWinlossModel.InsuranceGemsWinLoss,
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
            var netWinLossA = new PokerGamesWinLoss(5, 0, (decimal)0.2, (decimal)5.2);
            PokerGamesWinLoss netWinLossB = null;

            // Act
            var actualNetWinLoss = PokerGamesWinLoss.Add(netWinLossA, netWinLossB);

            // Assert
            var result = compareLogic.Compare(netWinLossA, actualNetWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidNetWinLoss_ReturnCorrectData()
        {
            // Arrange
            var netWinLossA = new PokerGamesWinLoss(5, 0, (decimal)0.2, (decimal)5.2);
            var netWinLossB = new PokerGamesWinLoss(5, 0, (decimal)0.2, (decimal)5.2);
            var expectedNetWinLoss = new PokerGamesWinLoss(10, 0, (decimal)0.4, (decimal)10.4);

            // Act
            var actualNetWinLoss = PokerGamesWinLoss.Add(netWinLossA, netWinLossB);

            // Assert
            var result = compareLogic.Compare(expectedNetWinLoss, actualNetWinLoss);
            Assert.True(result.AreEqual);
        }
    }
}