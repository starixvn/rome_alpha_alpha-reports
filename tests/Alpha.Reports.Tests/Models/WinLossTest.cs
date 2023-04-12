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
        public void Add_NullWinLoss_ReturnCurrentWinLoss()
        {
            // Arrange
            WinLossModel winLoss = null;
            var currentWinLoss = new WinLossModel();
            var expectedWinLoss = currentWinLoss;

            // Act
            var actuaWinLoss = currentWinLoss.Add(winLoss);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void Add_ValidWinLoss_ReturnCorrectData()
        {
            // Arrange
            var currentWinLoss = new WinLossModel(
                custId: 1265,
                custName: "UserNameA",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2), firstName: null, lastName: null);

            var winLoss = new WinLossModel(
                custId: 1266,
                custName: "UserNameB",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2), firstName: null, lastName: null);

            var expectedWinLoss = new WinLossModel(
                custId: 1265,
                custName: "UserNameA",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4), firstName: null, lastName: null);

            // Act
            var actuaWinLoss = currentWinLoss.Add(winLoss);

            // Assert
            var result = compareLogic.Compare(expectedWinLoss, actuaWinLoss);
            Assert.True(result.AreEqual);
        }
    }
}