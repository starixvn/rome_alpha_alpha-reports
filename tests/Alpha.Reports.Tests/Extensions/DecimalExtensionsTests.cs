namespace Alpha.Reports.Tests.Extensions
{
    using Alpha.Reports.Extensions;
    using Xunit;

    [Trait("Category", "Extensions")]
    public class DecimalExtensionsTests
    {
        [Theory]
        [InlineData(1.234, 2, 1.23)]
        [InlineData(1.235, 2, 1.23)]
        [InlineData(1.236, 2, 1.23)]
        [InlineData(1.299, 1, 1.2)]
        [InlineData(1.2345, 3, 1.234)]
        public void RoundDown_Always_ReturnRoundedValue(decimal input, int decimalPlaces, decimal output)
        {
            var actual = input.RoundDown(decimalPlaces);
            Assert.Equal(output, actual);
        }
    }
}
