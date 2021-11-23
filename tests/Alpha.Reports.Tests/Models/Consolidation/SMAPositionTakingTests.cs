namespace Alpha.Reports.Tests.Models.Consolidation
{
    using Alpha.Reports.Models.Consolidation;
    using Xunit;

    public class SMAPositionTakingTests
    {
        [Fact]
        public void Constructor_Default_ReturnsDefaultProperties()
        {
            // Arrange & Act
            var model = new SMAPositionTaking();

            // Assert
            Assert.True(0 == model.CustId
                && 0 == model.ValuePT
                && 0 == model.ProductId);
        }

        [Fact]
        public void Constructor_Always_ReturnsCorrectProperties()
        {
            // Arrange & Act
            var model = new SMAPositionTaking(123456, 56.79M, 23);

            // Assert
            Assert.True(123456 == model.CustId
                && 56.79M == model.ValuePT
                && 23 == model.ProductId);
        }
    }
}
