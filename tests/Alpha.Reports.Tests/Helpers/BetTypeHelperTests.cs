namespace Alpha.Reports.Tests.Helpers
{
    using Reports.Helpers;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class BetTypeHelperTests
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
        [InlineData(31)]
        [InlineData(32)]
        [InlineData(33)]
        [InlineData(41)]
        [InlineData(42)]
        [InlineData(43)]
        public void IsRacing_InRacingRange_ReturnsTrue(int betType)
        {
            // Act
            var actualIsRacing = BetTypeHelper.IsRacing(betType);

            // Assert
            Assert.True(actualIsRacing);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(30)]
        [InlineData(34)]
        [InlineData(40)]
        [InlineData(44)]
        [InlineData(int.MaxValue)]
        public void IsRacing_NotInRacingRange_ReturnsFalse(int betType)
        {
            // Act
            var actualIsRacing = BetTypeHelper.IsRacing(betType);

            // Assert
            Assert.False(actualIsRacing);
        }

        [Theory]
        [InlineData(1231)]
        [InlineData(1232)]
        [InlineData(1238)]
        public void IsVirtualHorseRacing_InVirtualHorseRacingRange_ReturnsTrue(int betType)
        {
            // Act
            var actualIsVirtualHorseRacing = BetTypeHelper.IsVirtualHorseRacing(betType);

            // Assert
            Assert.True(actualIsVirtualHorseRacing);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(1230)]
        [InlineData(1239)]
        [InlineData(int.MaxValue)]
        public void IsVirtualHorseRacing_NotInVirtualHorseRacingRange_ReturnsFalse(int betType)
        {
            // Act
            var actualIsVirtualHorseRacing = BetTypeHelper.IsVirtualHorseRacing(betType);

            // Assert
            Assert.False(actualIsVirtualHorseRacing);
        }
    }
}
