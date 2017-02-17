namespace Alpha.Reports.Tests.Models
{
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models;
    using Reports.Models.WinlossByGameType;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    public class WinLossByGameTypeModelTest
    {
        private readonly CompareLogic compareLogic;

        public WinLossByGameTypeModelTest()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void Add_NullWinLossByGameType_ReturnCurrentWinLossByGameType()
        {
            // Arrange
            WinLossByGameTypeModel winLossByGameType = null;
            var actualWinLossByGameType = new WinLossByGameTypeModel();
            var expectedWinLoss = actualWinLossByGameType;

            // Act
            actualWinLossByGameType.Add(winLossByGameType);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actualWinLossByGameType);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidWinLossByGameType_ReturnCorrectData()
        {
            // Arrange
            var actuaWinLossByGameTypeModel = new WinLossByGameTypeModel(
                gameTypeId: 2176,
                gameTypeName: "Sea Orchestra",
                turnover: (decimal)400.01,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var winLossByGameType = new WinLossByGameTypeModel(
                gameTypeId: 2301,
                gameTypeName: "(Mobile)Baccarat ",
                turnover: (decimal)400.00,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var expectedWinLoss = new WinLossByGameTypeModel(
                gameTypeId: 2176,
                gameTypeName: "Sea Orchestra",
                turnover: (decimal)800.01,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4));

            // Act
            actuaWinLossByGameTypeModel.Add(winLossByGameType);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLossByGameTypeModel);
            Assert.True(result.AreEqual);
        }
    }
}