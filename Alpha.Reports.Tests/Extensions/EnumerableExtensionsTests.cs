namespace Alpha.Reports.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using ModelsForTesting;
    using Reports.Extensions;
    using Reports.Models.WinlossDetails;

    public class EnumerableExtensionsTests
    {
        private List<WinLossDetail> winLossDetails;

        private List<WinLossDetail> descendingSortedListByWinLoss;

        private List<WinLossDetail> ascendingSortedListByWinLoss;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            winLossDetails = new List<WinLossDetail>();
            descendingSortedListByWinLoss = new List<WinLossDetail>();
            ascendingSortedListByWinLoss = new List<WinLossDetail>();

            var numberOfRecords = 3;
            for (var i = 1; i <= numberOfRecords; i++)
            {
                winLossDetails.Add(new WinLossDetailForTesting(
                    i,
                    $"Cust Name {i}",
                    i,
                    i,
                    i,
                    i,
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i)));

                ascendingSortedListByWinLoss.Add(new WinLossDetailForTesting(
                    i,
                    $"Cust Name {i}",
                    i,
                    i,
                    i,
                    i,
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i),
                    new NetWinLossForTesting(i, i)));

                var descendingValue = numberOfRecords - i + 1;
                descendingSortedListByWinLoss.Add(new WinLossDetailForTesting(
                    descendingValue,
                    $"Cust Name {descendingValue}",
                    descendingValue,
                    descendingValue,
                    descendingValue,
                    descendingValue,
                    new NetWinLossForTesting(descendingValue, descendingValue),
                    new NetWinLossForTesting(descendingValue, descendingValue),
                    new NetWinLossForTesting(descendingValue, descendingValue),
                    new NetWinLossForTesting(descendingValue, descendingValue)));
            }
        }

        [TestCase("CustId")]
        [TestCase("CustName")]
        [TestCase("Turnover")]
        [TestCase("NetTurnover")]
        [TestCase("GrossComm")]
        [TestCase("NumberOfTickets")]
        [TestCase("MemberNetWinLoss.WinLoss")]
        [TestCase("MemberNetWinLoss.Comm")]
        [TestCase("MemberNetWinLoss.Total")]
        [TestCase("AgentNetWinLoss.WinLoss")]
        [TestCase("AgentNetWinLoss.Comm")]
        [TestCase("AgentNetWinLoss.Total")]
        [TestCase("MasterNetWinLoss.WinLoss")]
        [TestCase("MasterNetWinLoss.Comm")]
        [TestCase("MasterNetWinLoss.Total")]
        [TestCase("SuperNetWinLoss.WinLoss")]
        [TestCase("SuperNetWinLoss.Comm")]
        [TestCase("SuperNetWinLoss.Total")]
        public void OrderBy_OrderByDescending_ReturnsDescendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = descendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName).ToList();

            // Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [TestCase("Company")]
        public void OrderBy_OrderByDescendingCompany_ReturnsDescendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = ascendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName).ToList();

            // Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [TestCase("CustId")]
        [TestCase("CustName")]
        [TestCase("Turnover")]
        [TestCase("NetTurnover")]
        [TestCase("GrossComm")]
        [TestCase("NumberOfTickets")]
        [TestCase("MemberNetWinLoss.WinLoss")]
        [TestCase("MemberNetWinLoss.Comm")]
        [TestCase("MemberNetWinLoss.Total")]
        [TestCase("AgentNetWinLoss.WinLoss")]
        [TestCase("AgentNetWinLoss.Comm")]
        [TestCase("AgentNetWinLoss.Total")]
        [TestCase("MasterNetWinLoss.WinLoss")]
        [TestCase("MasterNetWinLoss.Comm")]
        [TestCase("MasterNetWinLoss.Total")]
        [TestCase("SuperNetWinLoss.WinLoss")]
        [TestCase("SuperNetWinLoss.Comm")]
        [TestCase("SuperNetWinLoss.Total")]
        public void OrderBy_OrderByAscending_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = ascendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            Assert.AreEqual(expectedList, actualList);
        }

        [TestCase("Company")]
        public void OrderBy_OrderByAscendingCompany_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = descendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            Assert.AreEqual(expectedList, actualList);
        }
    }
}
