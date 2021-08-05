namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Models
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperPTModelTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var model = new SuperPTModel();

            // Assert
            Assert.True(0 == model.CustId
                && 0 == model.SuperPT
                && 0 == model.ProductId
                && null == model.ProductName);
        }

        [Fact]
        public void Constructor_Always_AssignsCorrectProperties()
        {
            // Arrange
            var custId = 123456;
            var superPT = 123.4M;
            var productId = 1;
            string productName = "Sportbooks";

            // Act
            var model = new SuperPTModel(custId, superPT, productId, productName);

            // Assert
            Assert.True(custId == model.CustId
                && superPT == model.SuperPT
                && productId == model.ProductId
                && productName == model.ProductName);
        }
    }
}
