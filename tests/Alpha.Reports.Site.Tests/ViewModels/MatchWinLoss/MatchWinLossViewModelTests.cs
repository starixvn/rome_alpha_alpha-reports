namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLoss
{
    using Extensions;
    using Site.ViewModels.MatchWinLoss;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    public class MatchWinLossViewModelTests
    {
        [Fact]
        public void Add_NullWinLossAAndNullWinLossB_ReturnsWinLossB()
        {
            // Arrange
            MatchWinLossViewModel winLossA = null;
            MatchWinLossViewModel winLossB = null;

            // Act
            var actualWinLoss = MatchWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            Assert.Null(actualWinLoss);
        }

        [Fact]
        public void Add_NullWinLossA_ReturnsWinLossB()
        {
            // Arrange
            MatchWinLossViewModel winLossA = null;
            var winLossB = new MatchWinLossViewModel("Event Info B", "Bet Type B", 1, 2, false, 0, 0, 0);

            // Act
            var actualWinLoss = MatchWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(winLossB, actualWinLoss);
        }

        [Fact]
        public void Add_NullWinLossB_ReturnsWinLossA()
        {
            // Arrange
            var winLossA = new MatchWinLossViewModel("Event Info A", "Bet Type A", 1, 2, false, 0, 0, 0);
            MatchWinLossViewModel winLossB = null;

            // Act
            var actualWinLoss = MatchWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(winLossA, actualWinLoss);
        }

        [Fact]
        public void Add_NotNullWinLossAAndNotNullWinLossB_ReturnsSumWinLoss()
        {
            // Arrange
            var winLossA = new MatchWinLossViewModel("Event Info A", "Bet Type A", 1, 2, false, 1, 2, 3);
            var winLossB = new MatchWinLossViewModel("Event Info B", "Bet Type B", 1, 2, false, 4, 5, 6);
            var expectedWinLoss = new MatchWinLossViewModel(string.Empty, string.Empty, 0, 0, false, 5, 7, 9);

            // Act
            var actualWinLoss = MatchWinLossViewModel.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedWinLoss, actualWinLoss);
        }
    }
}
