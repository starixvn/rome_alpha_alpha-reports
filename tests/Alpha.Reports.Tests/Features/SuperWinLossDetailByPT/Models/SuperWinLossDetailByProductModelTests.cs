namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Models
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Models;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperWinLossDetailByProductModelTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var model = new SuperWinLossDetailByProductModel();

            // Assert
            Assert.True(0 == model.ProductId
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
            int productId = 1;
            decimal turnover = 1;
            decimal netTurnover = 2;
            decimal grossComm = 3;
            decimal memberWinLost = 4;
            decimal memberComm = 5;
            decimal memberTotal = 6;
            decimal agWinLost = 7;
            decimal agComm = 8;
            decimal agTotal = 9;
            decimal mAgWinLost = 10;
            decimal mAgComm = 11;
            decimal mAgTotal = 12;
            decimal sAgWinLost = 13;
            decimal sAgComm = 14;
            decimal sAgTotal = 15;
            decimal company = 16;

            // Act
            var model = new SuperWinLossDetailByProductModel(
                productId,
                turnover,
                netTurnover,
                grossComm,
                memberWinLost,
                memberComm,
                memberTotal,
                agWinLost,
                agComm,
                agTotal,
                mAgWinLost,
                mAgComm,
                mAgTotal,
                sAgWinLost,
                sAgComm,
                sAgTotal,
                company);

            // Assert
            Assert.True(productId == model.ProductId
                && turnover == model.Turnover
                && netTurnover == model.NetTurnover
                && grossComm == model.GrossComm
                && memberWinLost == model.MemberWinLost
                && memberComm == model.MemberComm
                && memberTotal == model.MemberTotal
                && agWinLost == model.AgWinLost
                && agComm == model.AgComm
                && agTotal == model.AgTotal
                && mAgWinLost == model.MAgWinLost
                && mAgComm == model.MAgComm
                && mAgTotal == model.MAgTotal
                && sAgWinLost == model.SAgWinLost
                && sAgComm == model.SAgComm
                && sAgTotal == model.SAgTotal
                && company == model.Company);
        }
    }
}
