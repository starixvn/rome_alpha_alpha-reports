namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Services;
    using Xunit;

    [Collection("FanexCacheCollection")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.Reports.Services.Impl.ProductsForCustomer";

        private readonly IDynamicRepository dynamicRepository;
        private readonly FanexCacheFixture fanexCacheFixture;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>()
        {
            new Product(1, "Product 1"),
            new Product(2, "Product 2"),
            new Product(3, "Product 3")
        };

        private ProductServiceImpl productServiceImpl;

        public ProductServiceImplTests(FanexCacheFixture fanexCacheFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;
            Func<int, bool> canViewProduct = (productId) => true;
            Func<int, string> getProductName = (productId) => $"Product {productId}";
            dynamicRepository = Substitute.For<IDynamicRepository>();
            productServiceImpl = new ProductServiceImpl(dynamicRepository, canViewProduct, getProductName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void FilterProductIdsByCustomer_NullProductIdsAndInvalidCustId_ReturnsEmptyProductIds(int custId)
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = Enumerable.Empty<int>();
            productServiceImpl = new ProductServiceImpl(dynamicRepository, null, null);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void FilterProductIdsByCustomer_NullProductIdsAndValidCustId_ReturnsAllCustomerProductIds(int custId)
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = allCustomerProductIds.Select(product => product.Id);

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void FilterProductIdsByCustomer_EmptyProductIdsAndInvalidCustId_ReturnsEmptyProductIds(int custId)
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = Enumerable.Empty<int>();

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void FilterProductIdsByCustomer_EmptyProductIdsAndValidCustId_ReturnsAllCustomerProductIds(int custId)
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = allCustomerProductIds.Select(product => product.Id);

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void GetProductNames_NullProductIds_ReturnsEmptyNameList()
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductNames = Enumerable.Empty<string>();

            // Act
            var actualProductNames = productServiceImpl.GetProductNames(productIds);

            // Assert
            Assert.Equal(expectedProductNames, actualProductNames);
        }

        [Fact]
        public void GetProductNames_NotNullProductIds_ReturnsCorrectNameList()
        {
            // Arrange
            IEnumerable<int> productIds = new List<int> { 1, 2, 3 };
            var expectedProductNames = new List<string> { "Product 1", "Product 2", "Product 3" };

            // Act
            var actualProductNames = productServiceImpl.GetProductNames(productIds);

            // Assert
            Assert.Equal(expectedProductNames, actualProductNames);
        }
    }
}