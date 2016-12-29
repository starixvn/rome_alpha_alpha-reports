namespace Alpha.Reports.Tests.Models
{
    using Extensions;
    using Reports.Models.Winloss;
    using Reports.Models.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossTest
    {
        [Fact]
        public void Add_NullWinLossA_ReturnWinLossB()
        {
            // Arrange
            WinLoss winLossA = null;
            var winLossB = new WinLoss();
            var expectedWinLoss = winLossB;

            // Act
            var actuaWinLoss = WinLoss.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedWinLoss, actuaWinLoss);
        }

        [Fact]
        public void Add_NullWinLossB_ReturnWinLossA()
        {
            // Arrange
            WinLoss winLossB = null;
            var winLossA = new WinLoss();
            var expectedWinLoss = winLossA;

            // Act
            var actuaWinLoss = WinLoss.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedWinLoss, actuaWinLoss);
        }

        [Fact]
        public void Add_ValidWinLoss_ReturnCorrectData()
        {
            // Arrange
            var winLossA = new WinLoss(
                custId: 1265,
                custName: "UserNameA",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var winLossB = new WinLoss(
                custId: 1266,
                custName: "UserNameB",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(5, (decimal)0.2),
                superNetWinLoss: new NetWinLoss((decimal)5.2));

            var expectedWinLoss = new WinLoss(
                custId: 0,
                custName: "Total",
                grossComm: 0,
                turnover: 0,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: new NetWinLoss(10, (decimal)0.4),
                superNetWinLoss: new NetWinLoss((decimal)10.4));

            // Act
            var actuaWinLoss = WinLoss.Add(winLossA, winLossB);

            // Assert
            AssertExtension.Equal(expectedWinLoss, actuaWinLoss);
        }
    }
}