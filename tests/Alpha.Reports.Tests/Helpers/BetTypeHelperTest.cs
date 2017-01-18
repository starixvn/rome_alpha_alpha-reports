namespace Alpha.Reports.Tests.Helpers
{
    using Reports.Helpers;
    using Xunit;

    public class BetTypeHelperTest
    {
        [Theory]
        [InlineData(81)]
        [InlineData(100)]
        [InlineData(120)]
        public void IsNumberGame_InNumberGameRange_ReturnsTrue(int betType)
        {
            // Act
            var actualIsNumberGame = BetTypeHelper.IsNumberGame(betType);

            // Assert
            Assert.True(actualIsNumberGame);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(80)]
        [InlineData(121)]
        [InlineData(int.MaxValue)]
        public void IsNumberGame_NotInNumberGameRange_ReturnsFalse(int betType)
        {
            // Act
            var actualIsNumberGame = BetTypeHelper.IsNumberGame(betType);

            // Assert
            Assert.False(actualIsNumberGame);
        }

        [Theory]
        [InlineData(1101)]
        [InlineData(1102)]
        [InlineData(1104)]
        public void IsLiveCasino_InLiveCasinoRange_ReturnsTrue(int betType)
        {
            // Act
            var actualIsLiveCasino = BetTypeHelper.IsLiveCasino(betType);

            // Assert
            Assert.True(actualIsLiveCasino);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1100)]
        [InlineData(1105)]
        [InlineData(int.MaxValue)]
        public void IsNumberGame_NotInLiveCasinoRange_ReturnsFalse(int betType)
        {
            // Act
            var actualIsLiveCasino = BetTypeHelper.IsLiveCasino(betType);

            // Assert
            Assert.False(actualIsLiveCasino);
        }
    }
}
