namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLossDetail
{
    using Extensions;
    using Reports.Constants;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    public class BetTypeWinLossViewModelTests
    {
        [Fact]
        public void Create_FromMatchWinLossDetailModel_ReturnsCorrectViewModel()
        {
            // Arrange
            var winLoss = new MatchWinLossDetailModel(
                isParlay: false,
                matchId: 1,
                homeId: 1,
                awayId: 1,
                betType: 1,
                liveIndicator: false,
                leagueId: 1,
                leagueType:(byte)LeagueType.Self,
                race: 1,
                matchCode: "Match Code 1",
                isNeutral: false,
                showTime: "Show Time 1",
                turnover: 1,
                grossComm: 1,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: null,
                superNetWinLoss: null);
            var betTypeName = "Bet Type Name";
            var expectedViewModel = new BetTypeWinLossViewModel(
                1,
                "Bet Type Name",
                1,
                1,
                false,
                1,
                null,
                null,
                null,
                null,
                new NetWinLoss(0, 0));

            // Act
            var actualViewModel = new BetTypeWinLossViewModel(winLoss, betTypeName);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Theory]
        [InlineData(85, true, "Next ")]
        [InlineData(85, false, "Next ")]
        [InlineData(86, true, "Next ")]
        [InlineData(86, false, "FT. ")]
        [InlineData(100, true, "")]
        [InlineData(100, false, "")]
        public void Name_Always_ReturnsCorrectNameWithPrefix(
            int betType,
            bool liveIndicator,
            string expectedName)
        {
            // Arrange
            var viewModel = new BetTypeWinLossViewModel(betType, string.Empty, 0, 0, liveIndicator, 0, null, null, null, null, null);

            // Act
            var actualName = viewModel.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }

        [Theory]
        [InlineData(2799, false, " (Virtual Sports)")]
        public void Name_Always_ReturnsCorrectNameWithSuffix(
            int betType,
            bool liveIndicator,
            string expectedName)
        {
            // Arrange
            var viewModel = new BetTypeWinLossViewModel(betType, string.Empty, 0, 0, liveIndicator, 0, null, null, null, null, null);

            // Act
            var actualName = viewModel.Name;

            // Assert
            Assert.Equal(expectedName, actualName);
        }

        [Fact]
        public void Add_NullWinLoss_ReturnsCurrentWinLoss()
        {
            // Arrange
            var currentWinLoss = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, 0, null, null, null, null, null);
            BetTypeWinLossViewModel winLoss = null;
            var expectedTotal = currentWinLoss;

            // Act
            var actualTotal = currentWinLoss.Add(winLoss);

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }

        [Fact]
        public void Add_NotNullWinLoss_ReturnsCorrectTotal()
        {
            // Arrange
            var currentWinLoss = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, 0, null, null, null, null, null);
            var winLoss = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, 0, null, null, null, null, null);
            var expectedTotal = new BetTypeWinLossViewModel(1, "Bet Type Name", 2, 2, false, 0, null, null, null, null, null);

            // Act
            var actualTotal = currentWinLoss.Add(winLoss);

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }
    }
}
