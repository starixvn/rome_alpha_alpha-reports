namespace Alpha.Reports.Tests.Models
{
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeModelTest
    {
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeModelTest()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void Add_NullWinLossByBetType_ReturnCurrentWinLossByBetType()
        {
            // Arrange
            WinLossByBetTypeModel winLossByBetType = null;
            var actualWinLossByBetType = new WinLossByBetTypeModel();
            var expectedWinLossByBetType = actualWinLossByBetType;

            // Act
            actualWinLossByBetType.Add(winLossByBetType);

            // Assert
            var result = compareLogic.Compare(expectedWinLossByBetType, actualWinLossByBetType);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_NullWinLossByBetTypeB_ReturnWinLossByBetTypeA()
        {
            // Arrange
            WinLossByBetTypeModel winLossByBetType = null;
            var actualWinLossByBetType = new WinLossByBetTypeModel();
            var expectedWinLossByBetType = actualWinLossByBetType;

            // Act
            actualWinLossByBetType.Add(winLossByBetType);

            // Assert
            var result = compareLogic.Compare(expectedWinLossByBetType, actualWinLossByBetType);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidWinLossByBetType_ReturnCorrectData()
        {
            // Arrange
            var actuaWinLossByBetTypeModel = new WinLossByBetTypeModel(
                betTypeId: 1,
                betTypeName: "Baccarat",
                productId: 0,
                liveIndicator: false,
                betCount: 4,
                turnover: (decimal)400.01,
                grossComm: (decimal)2.46,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var winLossByBetType = new WinLossByBetTypeModel(
                betTypeId: 2,
                betTypeName: "Blackjack",
                productId: 0,
                liveIndicator: false,
                betCount: 2,
                turnover: (decimal)400.00,
                grossComm: (decimal)2.45,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var expectedWinLoss = new WinLossByBetTypeModel(
                betTypeId: 1,
                betTypeName: "Baccarat",
                productId: 0,
                liveIndicator: false,
                betCount: 6,
                turnover: (decimal)800.01,
                grossComm: (decimal)4.91,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4));

            // Act
            actuaWinLossByBetTypeModel.Add(winLossByBetType);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLossByBetTypeModel);
            Assert.True(result.AreEqual);
        }
    }
}