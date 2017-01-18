namespace Alpha.Reports.Tests.Models.MatchWinLoss
{
    using Fixtures;
    using Reports.Models.MatchWinLoss;
    using Xunit;

    [Trait("Category", "MatchWinLoss")]
    [Collection("AnyValueCreatorCollection")]
    public class MatchWinLossModelTest
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MatchWinLossModelTest(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void LiveIndicator_Always_ReturnsCorrectValue(bool liveIndicator)
        {
            // Arrange
            var expectLiveIndicator = liveIndicator;
            var matchWinLossModel = new MatchWinLossModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                liveIndicator,
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>());

            // Act
            var actualLiveIndicator = matchWinLossModel.LiveIndicator;

            // Assert
            Assert.Equal(expectLiveIndicator, actualLiveIndicator);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(1, 0, 1)]
        [InlineData(1, 1, 2)]
        [InlineData(0.1, 1.1, 1.2)]
        [InlineData(1.1, 0.1, 1.2)]
        [InlineData(1.1, 1.1, 2.2)]
        public void Total_Always_ReturnsCorrectValue(
            decimal commission,
            decimal positionTaking,
            decimal expectTotal)
        {
            // Arrange
            var matchWinLossModel = new MatchWinLossModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<bool>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                commission,
                positionTaking);

            // Act
            var actualTotal = matchWinLossModel.Total;

            // Assert
            Assert.Equal(expectTotal, actualTotal);
        }
    }
}
