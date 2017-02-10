namespace Alpha.Reports.Tests.Extensions
{
    using Reports.Extensions;
    using Xunit;

    [Trait("Category", "Extensions")]
    public class IntExtensionsTests
    {
        [Fact]
        public void IsBetween_InsideRange_ReturnsTrue()
        {
            // Arrange
            var value = 2;

            // Act
            var actualIsBetween = value.IsBetween(1, 3);

            // Assert
            Assert.True(actualIsBetween);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(int.MaxValue)]
        public void IsBetween_OutsideRange_ReturnsFalse(int value)
        {
            // Act
            var actualIsBetween = value.IsBetween(2, 4);

            // Assert
            Assert.False(actualIsBetween);
        }
    }
}
