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
    }
}
