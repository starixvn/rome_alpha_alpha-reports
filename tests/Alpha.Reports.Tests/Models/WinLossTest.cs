namespace Alpha.Reports.Tests.Models
{
    using KellermanSoftware.CompareNetObjects;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossTest
    {
        private readonly CompareLogic compareLogic;

        public WinLossTest()
        {
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void Add_NullWinLossA_ReturnWinLossB()
        {
            // Arrange
            WinLossModel winLossA = null;
            var winLossB = new WinLossModel();
            var expectedWinLoss = winLossB;

            // Act
            var actuaWinLoss = WinLossModel.Add(winLossA, winLossB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_NullWinLossB_ReturnWinLossA()
        {
            // Arrange
            WinLossModel winLossB = null;
            var winLossA = new WinLossModel();
            var expectedWinLoss = winLossA;

            // Act
            var actuaWinLoss = WinLossModel.Add(winLossA, winLossB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidWinLoss_ReturnCorrectData()
        {
            // Arrange
            var winLossA = new WinLossModel(
                custId: 1265,
                custName: "UserNameA",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var winLossB = new WinLossModel(
                custId: 1266,
                custName: "UserNameB",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var expectedWinLoss = new WinLossModel(
                custId: 0,
                custName: "Total",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4));

            // Act
            var actuaWinLoss = WinLossModel.Add(winLossA, winLossB);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }
    }
}