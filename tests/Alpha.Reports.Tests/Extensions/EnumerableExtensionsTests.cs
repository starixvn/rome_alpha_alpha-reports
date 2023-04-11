namespace Alpha.Reports.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using TestCases;
    using Xunit;

    public class EnumerableExtensionsTests
    {
        private readonly List<WinLossDetailModel> winLossDetails;

        private readonly List<WinLossDetailModel> descendingSortedListByWinLoss;

        private readonly List<WinLossDetailModel> ascendingSortedListByWinLoss;

        public EnumerableExtensionsTests()
        {
            winLossDetails = new List<WinLossDetailModel>();
            descendingSortedListByWinLoss = new List<WinLossDetailModel>();
            ascendingSortedListByWinLoss = new List<WinLossDetailModel>();

            var numberOfRecords = 3;
            for (var i = 1; i <= numberOfRecords; i++)
            {
                winLossDetails.Add(new WinLossDetailModel(
                    i,
                    $"Cust Name {i}",
                    string.Empty,
                    string.Empty,
                    i,
                    i,
                    i,
                    i,
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i)));

                ascendingSortedListByWinLoss.Add(new WinLossDetailModel(
                    i,
                    $"Cust Name {i}",
                    string.Empty,
                    string.Empty,
                    i,
                    i,
                    i,
                    i,
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i),
                    new NetWinLoss(i, i)));

                var descendingValue = numberOfRecords - i + 1;
                descendingSortedListByWinLoss.Add(new WinLossDetailModel(
                    descendingValue,
                    $"Cust Name {descendingValue}",
                    string.Empty,
                    string.Empty,
                    descendingValue,
                    descendingValue,
                    descendingValue,
                    descendingValue,
                    new NetWinLoss(descendingValue, descendingValue),
                    new NetWinLoss(descendingValue, descendingValue),
                    new NetWinLoss(descendingValue, descendingValue),
                    new NetWinLoss(descendingValue, descendingValue)));
            }
        }

        [Theory]
        [MemberData("AllProperties", MemberType = typeof(WinLossDetailPropertyTestCases))]
        public void OrderBy_OrderByDescending_ReturnsDescendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = descendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName).ToList();

            // Assert
            AssertExtension.Equal(expectedList, actualList);
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
            AssertExtension.Equal(expectedList, actualList);
        }

        [Theory]
        [MemberData("AllProperties", MemberType = typeof(WinLossDetailPropertyTestCases))]
        public void OrderBy_OrderByAscending_ReturnsAscendingOrder(string propertyName)
        {
            // Arrange
            var expectedList = ascendingSortedListByWinLoss;

            // Act
            var actualList = winLossDetails.OrderBy(propertyName, descending: false).ToList();

            // Assert
            AssertExtension.Equal(expectedList, actualList);
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
            AssertExtension.Equal(expectedList, actualList);
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
            AssertExtension.Equal(expectedList, actualList);
        }

        [Theory]
        [InlineData(null)]
        public void OrderBy_OrderByNullPropertyName_ThrowsNullReferenceException(string propertyName)
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => winLossDetails.OrderBy(propertyName, descending: false).ToList());
        }

        [Fact]
        public void Exclude_EmptyExcludeProducts_ReturnsFullSource()
        {
            // Arrange
            var excludeProducts = Enumerable.Empty<Product>();
            var source = new List<Product> { new Product() };

            // Act
            var actualList = EnumerableExtensions.Exclude(source, excludeProducts);

            // Assert
            AssertExtension.Equal(source, actualList);
        }

        [Fact]
        public void Exclude_NullExcludeProducts_ReturnsFullSource()
        {
            // Arrange
            IEnumerable<Product> excludeProducts = null;
            var source = new List<Product> { new Product() };

            // Act
            var actualList = EnumerableExtensions.Exclude(source, excludeProducts);

            // Assert
            AssertExtension.Equal(source, actualList);
        }

        [Fact]
        public void Exclude_NullExcludeProducts_ReturnsCorrectData()
        {
            // Arrange
            var excludeProducts = new List<Product>
            {
                new Product(2, "Product 2", null, 0, 0),
                new Product(5, "Product 5", null, 0, 0)
            };

            var source = new List<Product>
            {
                new Product(1, "Product 1", null, 0, 0),
                new Product(2, "Product 2", null, 0, 0),
                new Product(4, "Product 4", null, 0, 0),
                new Product(5, "Product 5", null, 0, 0)
            };

            var expectedList = new List<Product>
            {
                new Product(1, "Product 1", null, 0, 0),
                new Product(4, "Product 4", null, 0, 0)
            };

            // Act
            var actualList = EnumerableExtensions.Exclude(source, excludeProducts).ToList();

            // Assert
            AssertExtension.Equal(expectedList, actualList);
        }
    }
}
