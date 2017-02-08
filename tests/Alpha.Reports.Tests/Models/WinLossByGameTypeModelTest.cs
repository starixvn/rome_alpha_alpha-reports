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
        public void Add_NullWinLossByGameTypeA_ReturnWinLossByGameTypeB()
        {
            // Arrange
            WinLossByGameTypeModel winLossByGameTypeA = null;
            var winLossByGameTypeB = new WinLossByGameTypeModel();
            var expectedWinLoss = winLossByGameTypeB;

            // Act
            var actuaWinLoss = WinLossByGameTypeModel.Add(winLossByGameTypeA, winLossByGameTypeB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_NullWinLossByGameTypeB_ReturnWinLossByGameTypeA()
        {
            // Arrange
            WinLossByGameTypeModel winLossByGameTypeB = null;
            var winLossByGameTypeA = new WinLossByGameTypeModel();
            var expectedWinLoss = winLossByGameTypeA;

            // Act
            var actuaWinLoss = WinLossByGameTypeModel.Add(winLossByGameTypeA, winLossByGameTypeB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidWinLossByGameType_ReturnCorrectData()
        {
            // Arrange
            var winLossByGameTypeA = new WinLossByGameTypeModel(
                gameTypeId: 2176,
                gameTypeName: "Sea Orchestra",
                turnover: (decimal)400.01,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var winLossByGameTypeB = new WinLossByGameTypeModel(
                gameTypeId: 2301,
                gameTypeName: "(Mobile)Baccarat ",
                turnover: (decimal)400.00,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var expectedWinLoss = new WinLossByGameTypeModel(
                gameTypeId: 0,
                gameTypeName: string.Empty,
                turnover: (decimal)800.01,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4));

            // Act
            var actuaWinLoss = WinLossByGameTypeModel.Add(winLossByGameTypeA, winLossByGameTypeB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }
    }
}