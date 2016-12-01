namespace Alpha.Reports.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ModelsForTesting;
    using Reports.Extensions;
    using Reports.Models.WinlossDetails;
    using Xunit;

    public class EnumerableExtensionsTests
    {
        private List<WinLossDetail> winLossDetails;

        private List<WinLossDetail> descendingSortedListByWinLoss;

        private List<WinLossDetail> ascendingSortedListByWinLoss;

        public EnumerableExtensionsTests()
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

        [Theory]
        [InlineData("CustId")]
        [InlineData("CustName")]
        [InlineData("Turnover")]
        [InlineData("NetTurnover")]
        [InlineData("GrossComm")]
        [InlineData("NumberOfTickets")]
        [InlineData("MemberNetWinLoss.WinLoss")]
        [InlineData("MemberNetWinLoss.Comm")]
        [InlineData("MemberNetWinLoss.Total")]
        [InlineData("AgentNetWinLoss.WinLoss")]
        [InlineData("AgentNetWinLoss.Comm")]
        [InlineData("AgentNetWinLoss.Total")]
        [InlineData("MasterNetWinLoss.WinLoss")]
        [InlineData("MasterNetWinLoss.Comm")]
        [InlineData("MasterNetWinLoss.Total")]
        [InlineData("SuperNetWinLoss.WinLoss")]
        [InlineData("SuperNetWinLoss.Comm")]
        [InlineData("SuperNetWinLoss.Total")]
        public void OrderBy_OrderByDescending_ReturnsDescendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = descendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName).ToList();

            // Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData("Company")]
        public void OrderBy_OrderByDescendingCompany_ReturnsDescendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = ascendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName).ToList();

            // Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData("CustId")]
        [InlineData("CustName")]
        [InlineData("Turnover")]
        [InlineData("NetTurnover")]
        [InlineData("GrossComm")]
        [InlineData("NumberOfTickets")]
        [InlineData("MemberNetWinLoss.WinLoss")]
        [InlineData("MemberNetWinLoss.Comm")]
        [InlineData("MemberNetWinLoss.Total")]
        [InlineData("AgentNetWinLoss.WinLoss")]
        [InlineData("AgentNetWinLoss.Comm")]
        [InlineData("AgentNetWinLoss.Total")]
        [InlineData("MasterNetWinLoss.WinLoss")]
        [InlineData("MasterNetWinLoss.Comm")]
        [InlineData("MasterNetWinLoss.Total")]
        [InlineData("SuperNetWinLoss.WinLoss")]
        [InlineData("SuperNetWinLoss.Comm")]
        [InlineData("SuperNetWinLoss.Total")]
        public void OrderBy_OrderByAscending_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = ascendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData("Company")]
        public void OrderBy_OrderByAscendingCompany_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = descendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("WrongPropertyName")]
        public void OrderBy_OrderByWrongPropertyName_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = winLossDetails;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            Assert.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData(null)]
        public void OrderBy_OrderByNullPropertyName_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = winLossDetails;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => winLossDetails.OrderBy(propertyName, descending: false).ToList());
        }
    }
}
