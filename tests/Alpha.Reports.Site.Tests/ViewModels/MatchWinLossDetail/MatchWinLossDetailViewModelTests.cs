namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLossDetail
{
    using System.Collections.Generic;
    using Fixtures;
    using Site.ViewModels.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    public class MatchWinLossDetailViewModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossDetailViewModelTests()
        {
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void RenderSubTotal_NotHaveMatchId_ReturnsFalse()
        {
            // Arrange
            var matchId = 0;
            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                matchId,
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.False(actualRenderSubTotal);
        }

        [Fact]
        public void RenderSubTotal_NotHaveRaceId_ReturnsFalse()
        {
            // Arrange
            var raceId = 0;
            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                raceId,
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.False(actualRenderSubTotal);
        }

        [Fact]
        public void RenderSubTotal_NotHaveRaceId_ReturnsTrue()
        {
            // Arrange
            var raceId = 1;
            var matchId = 1;

            var model = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                matchId,
                anyValueCreatorFixture.Create<string>(),
                raceId,
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>());

            // Act
            var actualRenderSubTotal = model.RenderSubTotal;

            // Assert
            Assert.True(actualRenderSubTotal);
        }
    }
}
