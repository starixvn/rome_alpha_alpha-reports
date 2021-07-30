namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Models
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailModelTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var model = new SuperWinLossDetailModel();

            // Assert
            Assert.True(0 == model.ProductId
                && null == model.ProductName
                && 0 == model.PositionTaking
                && 0 == model.Turnover
                && 0 == model.NetTurnover
                && 0 == model.GrossComm
                && 0 == model.MemberWinLost
                && 0 == model.MemberComm
                && 0 == model.MemberTotal
                && 0 == model.AgWinLost
                && 0 == model.AgComm
                && 0 == model.AgTotal
                && 0 == model.MAgWinLost
                && 0 == model.MAgComm
                && 0 == model.MAgTotal
                && 0 == model.SAgWinLost
                && 0 == model.SAgComm
                && 0 == model.SAgTotal
                && 0 == model.Company);
        }

        [Fact]
        public void Constructor_Always_AssignsCorrectProperties()
        {
            // Arrange
            var superWinLossDetailByProductModel = new SuperWinLossDetailByProductModel(1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            var superPTModel = new SuperPTModel(123456, 123.4M, 1, "Sportbooks");

            // Act
            var model = new SuperWinLossDetailModel(superWinLossDetailByProductModel, superPTModel);

            // Assert
            Assert.True(superPTModel.ProductId == model.ProductId
                && superPTModel.ProductName == model.ProductName
                && superPTModel.SuperPT == model.PositionTaking
                && superWinLossDetailByProductModel.Turnover == model.Turnover
                && superWinLossDetailByProductModel.NetTurnover == model.NetTurnover
                && superWinLossDetailByProductModel.GrossComm == model.GrossComm
                && superWinLossDetailByProductModel.MemberWinLost == model.MemberWinLost
                && superWinLossDetailByProductModel.MemberComm == model.MemberComm
                && superWinLossDetailByProductModel.MemberTotal == model.MemberTotal
                && superWinLossDetailByProductModel.AgWinLost == model.AgWinLost
                && superWinLossDetailByProductModel.AgComm == model.AgComm
                && superWinLossDetailByProductModel.AgTotal == model.AgTotal
                && superWinLossDetailByProductModel.MAgWinLost == model.MAgWinLost
                && superWinLossDetailByProductModel.MAgComm == model.MAgComm
                && superWinLossDetailByProductModel.MAgTotal == model.MAgTotal
                && superWinLossDetailByProductModel.SAgWinLost == model.SAgWinLost
                && superWinLossDetailByProductModel.SAgComm == model.SAgComm
                && superWinLossDetailByProductModel.SAgTotal == model.SAgTotal
                && superWinLossDetailByProductModel.Company == model.Company);
        }

        [Fact]
        public void Add_Always_ReturnsCorrectTotal()
        {
            // Arrange
            var model = new SuperWinLossDetailModel(
                new SuperWinLossDetailByProductModel(1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
                new SuperPTModel());

            // Act
            var total = model.Add(new SuperWinLossDetailModel(
                new SuperWinLossDetailByProductModel(1, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16),
                new SuperPTModel()));

            // Assert
            Assert.True(2 == total.Turnover
                && 4 == total.NetTurnover
                && 6 == total.GrossComm
                && 8 == total.MemberWinLost
                && 10 == total.MemberComm
                && 12 == total.MemberTotal
                && 14 == total.AgWinLost
                && 16 == total.AgComm
                && 18 == total.AgTotal
                && 20 == total.MAgWinLost
                && 22 == total.MAgComm
                && 24 == total.MAgTotal
                && 26 == total.SAgWinLost
                && 28 == total.SAgComm
                && 30 == total.SAgTotal
                && 32 == total.Company);
        }
    }
}
