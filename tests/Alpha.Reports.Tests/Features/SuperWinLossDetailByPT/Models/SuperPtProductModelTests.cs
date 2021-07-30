namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Models
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperPtProductModelTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectProperties()
        {
            // Arrange
            var positionTaking = 123.4M;
            string productName = "Sportbooks";

            // Act
            var model = new SuperPtProductModel(positionTaking, productName);

            // Assert
            Assert.True(positionTaking == model.PositionTaking
                && productName == model.ProductName);
        }
    }
}
