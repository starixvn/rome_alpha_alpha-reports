namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Services.Impl;
    using Xunit;

    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.Reports.Services.Impl.ProductsForCustomer";

        private readonly Func<int, bool> canViewProduct;
        private readonly Func<int, string> getProductName;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ProductServiceImpl productServiceImpl;
        private readonly Cache cacheForTesting;
        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>()
        {
            new Product(1, "Product 1"),
            new Product(2, "Product 2"),
            new Product(3, "Product 3")
        };

        public ProductServiceImplTests()
        {
            canViewProduct = (productId) => true;
            getProductName = (productId) => string.Empty;
            dynamicRepository = Substitute.For<IDynamicRepository>();
            productServiceImpl = new ProductServiceImpl(dynamicRepository, canViewProduct, getProductName);

            cacheForTesting = Substitute.For<Cache>();
            CacheService.SwitchCacheStorage(cacheForTesting);
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

            var cachedProducts = Enumerable.Empty<Product>();
            cacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

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

            var cachedProducts = Enumerable.Empty<Product>();
            cacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }
    }
}
