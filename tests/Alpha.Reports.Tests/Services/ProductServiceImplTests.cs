namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexCacheCollection")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.Reports.Services.Impl.ProductsForCustomer";
        private const string ProductsNeedDelayTimeToSumarizeDataCacheKey = "Alpha.Reports.Services.Impl.ProductsNeedDelayTimeToSumarizeData";

        private readonly IDynamicRepository dynamicRepository;
        private readonly FanexCacheFixture fanexCacheFixture;
        private readonly CompareLogic compareLogic;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>()
        {
            new Product(1, "Product 1", null, 0, 0),
            new Product(2, "Product 2", null, 0, 0),
            new Product(3, "Product 3", null, 0, 0)
        };

        private ProductServiceImpl productServiceImpl;
        private IEnumerable<ProductsByRefTable> productsNeedDelayTimeToSumarizeDataInCache;

        public ProductServiceImplTests(FanexCacheFixture fanexCacheFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;
            Func<int, bool> canViewProduct = (productId) => true;
            Func<int, string> getProductName = (productId) => $"Product {productId}";
            dynamicRepository = Substitute.For<IDynamicRepository>();
            productServiceImpl = new ProductServiceImpl(dynamicRepository, canViewProduct, getProductName);

            dynamicRepository.Fetch<ProductsByRefTable>(Arg.Any<GetProductsByRefTableCriteria>()).Returns(new List<ProductsByRefTable>
            {
                new ProductsByRefTable(1, 2),
                new ProductsByRefTable(2, 2)
            });

            IEnumerable<ProductsByRefTable> productsNeedDelayTimeToSumarizeData;

            fanexCacheFixture.CacheForTesting.TryGet(ProductsNeedDelayTimeToSumarizeDataCacheKey, out productsNeedDelayTimeToSumarizeData).Returns(x =>
            {
                x[1] = productsNeedDelayTimeToSumarizeDataInCache;

                return productsNeedDelayTimeToSumarizeDataInCache != null;
            });

            ConfiguredCacheService.Set(ProductsNeedDelayTimeToSumarizeDataCacheKey, productsNeedDelayTimeToSumarizeDataInCache);

            compareLogic = new CompareLogic();
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
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, "", 1, productIds);

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
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, "", 1, productIds);

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
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, "", 1, productIds);

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
            var expectedProductIds = new List<int> { 1, 2, 3 };

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, "", 1, productIds);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void FilterProductIdsByCustomer_HasDataProductIdsAndValidCustId_ReturnsCorrectValue()
        {
            // Arrange
            var productIds = new List<int> { 1, 3, 5 };
            var expectedProductIds = new List<int> { 1, 3 };
            var custId = 3;

            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);

            // Act
            var actualProductIds = productServiceImpl.FilterProductIdsByCustomer(custId, "", 1, productIds);

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void ConcatProductNames_NullOrEmptyProductIds_ReturnsEmpty(string productIds)
        {
            // Act
            var actualProductNames = productServiceImpl.ConcatProductNames(productIds);

            // Assert
            Assert.Empty(actualProductNames);
        }

        [Fact]
        public void ConcatProductNames_NotNullOrEmptyProductIds_ReturnsCorrectNames()
        {
            // Arrange
            var productIds = "1,2,3";
            var expectedProductNames = "Product 1, Product 2, Product 3";

            // Act
            var actualProductNames = productServiceImpl.ConcatProductNames(productIds);

            // Assert
            Assert.Equal(expectedProductNames, actualProductNames);
        }

        [Theory]
        [InlineData("1,2,3,5", "1,2,3,5")]
        [InlineData("1,2,3,5,6", "1,2,3,5")]
        [InlineData("1,2,3,5,7,8", "1,2,3,5")]
        [InlineData("1,2,5", "1,2,5")]
        [InlineData("1", "1")]
        [InlineData("", "")]
        public void FilterProductsByCustomer_Always_FiltersSelectedProductsWithData(
            string productIds,
            string expectProductIds)
        {
            // Arrange
            var custId = 2;
            var customerProductIds = "1,2,3,5";
            IEnumerable<Product> customerProducts = customerProductIds.ToListInt().Select(id => new Product(id, string.Empty, null, 0, 0));
            IEnumerable<Product> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(x =>
            {
                x[1] = customerProducts;
                return true;
            });

            // Act
            var actualProducts = productServiceImpl.FilterProductsByCustomer(custId, "", 1, productIds.ToListInt());

            // Assert
            AssertExtension.Equal(expectProductIds.ToListInt(), actualProducts.Select(product => product.Id).ToList());
        }

        [Fact]
        public void GetProductsNeedDelayTimeToSumarizeDataByCustomer_Always_ReturnsCorrectProducts()
        {
            // Arrange
            productsNeedDelayTimeToSumarizeDataInCache = null;
            var expectProducts = new List<byte> { 1, 2 };

            // Act
            var actualProducts = productServiceImpl.GetProductsNeedDelayTimeToSumarizeDataByCustomer(1, string.Empty, 1);

            // Assert
            Assert.Equal(expectProducts, actualProducts);
        }

        [Fact]
        public void IsShowProduct_NotHaveProduct_ReturnsFalse()
        {
            // Arrange
            var cachedProducts = new List<Product>
            {
                new Product(1, string.Empty, null, 0, 0)
            };

            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-1", out cachedProducts).Returns(true);

            // Act
            var actualValue = productServiceImpl.IsShowProduct(1, "", 1, 33);

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void IsShowProduct_HaveProduct_ReturnsFalse()
        {
            // Arrange
            var cachedProducts = new List<Product>
            {
                new Product(33, string.Empty, null, 0, 0)
            };

            fanexCacheFixture.CacheForTesting.TryGet($"{ProductCacheKey}-2", out cachedProducts).Returns(true);

            // Act
            var actualValue = productServiceImpl.IsShowProduct(2, "", 1, 33);

            // Assert
            Assert.False(actualValue);
        }

        [Fact]
        public void CanViewBySite_NonOfferedProduct_ReturnFalse()
        {
            // Arrange
            var site = "IBCINdo";
            var products = new List<ProductBySiteModel>() { new ProductBySiteModel(22), new ProductBySiteModel(23) };
            IEnumerable<ProductBySiteModel> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"Alpha.Reports.Services.Impl.ProductsBySite-{site}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<ProductBySiteModel>(Arg.Any<GetOfferedProductsBySiteCriteria>()).Returns(products);

            // Act
            var act = productServiceImpl.CanViewBySite(1, site, 1, 21);

            // Assert
            Assert.False(act);
        }

        [Fact]
        public void CanViewBySite_OfferedProduct_ReturnTrue()
        {
            // Arrange
            var site = "IBCINdo";
            var products = new List<ProductBySiteModel>() { new ProductBySiteModel(22), new ProductBySiteModel(23) };
            IEnumerable<ProductBySiteModel> cachedProducts;
            fanexCacheFixture.CacheForTesting.TryGet($"Alpha.Reports.Services.Impl.ProductsBySite-{site}", out cachedProducts).Returns(false);

            dynamicRepository.Fetch<ProductBySiteModel>(Arg.Any<GetOfferedProductsBySiteCriteria>()).Returns(products);

            // Act
            var act = productServiceImpl.CanViewBySite(1, site, 1, 23);

            // Assert
            Assert.True(act);
        }

        [Theory]
        [InlineData(0, "ibcindo", 0)]
        [InlineData(1, "best-odds", 1)]
        public void OfferedProduct_Properties_NotNull(int custId, string site, int currency)
        {
            // Arrange
            var act = new GetOfferedProductsBySiteCriteria(custId, site, currency);

            // Assert
            Assert.NotNull(act.CustId);

            // Assert
            Assert.NotNull(act.Site);

            // Assert
            Assert.NotNull(act.CurrencyId);
        }

        [Fact]
        public void OfferedProduct_SettingStatus_GreaterThanOrEqualsToZero()
        {
            // Arrange
            var act = new GetOfferedProductsBySiteCriteria(0, "abc", 20);

            // Assert
            Assert.True(act.SettingStatus >= 0);
        }

        [Fact]
        public void OfferedProduct_SpKey_AlwaysEqualsToAcc_CustSetting_CheckMacauEnable()
        {
            // Arrange
            var act = new GetOfferedProductsBySiteCriteria(0, "abc", 20);

            // Assert
            Assert.True(act.GetSettingKey() == "Acc_CustSetting_CheckMacauEnable");
        }

        [Fact]
        public void OfferedProduct_IsValid_AlwaysTrue()
        {
            // Arrange
            var act = new GetOfferedProductsBySiteCriteria(0, "abc", 20);

            // Assert
            Assert.True(act.IsValid());
        }

        [Fact]
        public void ProductBySiteModel_DefaultCtor_AlwaysZeroForProductId()
        {
            // Arrange
            var act = new ProductBySiteModel();

            // Assert
            Assert.True(act.ProductId == 0);
        }

        [Fact]
        public void ProductDetail_DefaultCtor_AlwaysZeroOrNullForProperties()
        {
            // Arrange
            var act = new ProductDetail();

            // Assert
            Assert.True(act.ProductId == 0);

            // Assert
            Assert.True(act.ResourceId == 0);

            // Assert
            Assert.True(act.SportType == 0);

            // Assert
            Assert.True(act.BetType == 0);

            // Assert
            Assert.True(act.Order == 0);

            // Assert
            Assert.Null(act.DisplayName);

            // Assert
            Assert.Null(act.RefId);
        }

        [Fact]
        public void CurrencyModel_DefaultCtor_AlwaysZeroOrNullForProperties()
        {
            // Arrange
            var act = new CurrencyModel();

            // Assert
            Assert.True(act.Id == 0);

            // Assert
            Assert.Null(act.Name);
        }

        [Fact]
        public void StatusModel_DefaultCtor_AlwaysZeroOrNullForProperties()
        {
            // Arrange
            var act = new StatusModel();

            // Assert
            Assert.True(act.Id == 0);

            // Assert
            Assert.Null(act.Name);
        }

        [Fact]
        public void TimeRange_DefaultCtor_AlwaysZeroOrNullForProperties()
        {
            // Arrange
            var act = new TimeRange();

            // Assert
            Assert.NotNull(act.FromDate);

            // Assert
            Assert.NotNull(act.ToDate);
        }
    }
}