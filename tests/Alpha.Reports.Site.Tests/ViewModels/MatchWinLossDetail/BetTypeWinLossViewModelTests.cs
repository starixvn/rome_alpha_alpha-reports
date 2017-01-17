namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLossDetail
{
    using Extensions;
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
                race: 1,
                matchCode: "Match Code 1",
                isNeutral: false,
                showTime: "Show Time 1",
                turnover: 1,
                grossComm: 1,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: null,
                superNetWinLoss: null,
                companyNetWinLoss: null);
            var betTypeName = "Bet Type Name";
            var expectedViewModel = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, null, null, null, null, null);

            // Act
            var actualViewModel = new BetTypeWinLossViewModel(winLoss, betTypeName);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void Add_NullWinLossA_ReturnsWinLossB()
        {
            // Arrange
            BetTypeWinLossViewModel winLossA = null;
            var winLossB = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, null, null, null, null, null);
            var expectedTotal = winLossB;

            // Act
            var actualTotal = BetTypeWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }

        [Fact]
        public void Add_NullWinLossB_ReturnsWinLossA()
        {
            // Arrange
            BetTypeWinLossViewModel winLossB = null;
            var winLossA = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, null, null, null, null, null);
            var expectedTotal = winLossA;

            // Act
            var actualTotal = BetTypeWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }

        [Fact]
        public void Add_NotNullWinLoss_ReturnsWinLossA()
        {
            // Arrange
            var winLossA = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, null, null, null, null, null);
            var winLossB = new BetTypeWinLossViewModel(1, "Bet Type Name", 1, 1, false, null, null, null, null, null);
            var expectedTotal = new BetTypeWinLossViewModel(0, "Sub Total", 2, 2, false, null, null, null, null, null);

            // Act
            var actualTotal = BetTypeWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedTotal, actualTotal);
        }
    }
}
