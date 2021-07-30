namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Models
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailByPTModelTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectProperties()
        {
            // Arrange
            var positionTaking = 123.4M;
            var superWinLossDetailByProductModel = new SuperWinLossDetailByProductModel();
            var superPTModel = new SuperPTModel();
            var superWinLossDetails = new[] { new SuperWinLossDetailModel(superWinLossDetailByProductModel, superPTModel) };

            // Act
            var model = new SuperWinLossDetailByPTModel(positionTaking, superWinLossDetails);

            // Assert
            Assert.True(positionTaking == model.PositionTaking
                && "12,340.0% {0}" == model.PositionTakingText
                && superWinLossDetails == model.SuperWinLossDetails);
        }
    }
}
