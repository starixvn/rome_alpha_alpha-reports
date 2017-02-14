namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLoss
{
    using Extensions;
    using Site.ViewModels.MatchWinLoss;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    public class MatchWinLossViewModelTests
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

        [Fact]
        public void Add_NotNullWinLoss_ReturnsSumWinLoss()
        {
            // Arrange
            var winLossA = new MatchWinLossViewModel("Event Info A", "Bet Type A", 1, 2, false, 1, 2, 3);
            var winLossB = new MatchWinLossViewModel("Event Info B", "Bet Type B", 1, 2, false, 4, 5, 6);
            var expectedWinLoss = new MatchWinLossViewModel("Event Info A", "Bet Type A", 1, 2, false, 5, 7, 9);

            // Act
            var actualWinLoss = winLossA.Add(winLossB);

            // Assert
            AssertExtension.Equal(expectedWinLoss, actualWinLoss);
        }
    }
}
