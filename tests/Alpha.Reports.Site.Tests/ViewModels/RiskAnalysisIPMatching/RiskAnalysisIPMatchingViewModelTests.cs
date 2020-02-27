namespace Alpha.Reports.Site.Tests.ViewModels.RiskAnalysisIPMatching
{
    using Extensions;
    using Site.ViewModels.MatchWinLoss;
    using Xunit;

    [Trait("Category", "RiskAnalysisIPMatching")]
    public class RiskAnalysisIPMatchingViewModelTests
    {
        [Fact]
        public void Add_NullWinLoss_ReturnsOldValue()
        {
            // Arrange
            var winLossA = new MatchWinLossViewModel("Event Info A", "Bet Type A", 1, 2, false, 0, 0, 0);
            MatchWinLossViewModel winLossB = null;

            // Act
            var actualWinLoss = winLossA.Add(winLossB);

            // Assert
            AssertExtension.Equal(winLossA, actualWinLoss);
        }
    }
}
