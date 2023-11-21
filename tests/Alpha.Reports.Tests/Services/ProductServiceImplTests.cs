namespace Alpha.Reports.Tests.Services
{
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class ProductServiceImplTests
    {
        private const string ProductCacheKey = "Alpha.Reports.Services.Impl.ProductsForCustomer";
        private const string ProductsNeedDelayTimeToSumarizeDataCacheKey = "Alpha.Reports.Services.Impl.ProductsNeedDelayTimeToSumarizeData";

        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly CompareLogic compareLogic;

        private readonly IEnumerable<Product> allCustomerProductIds = new List<Product>()
        {
            new Product(1, "Product 1", null, 0, 0),
            new Product(2, "Product 2", null, 0, 0),
            new Product(3, "Product 3", null, 0, 0)
        };

        private ProductServiceImpl productServiceImpl;
        private IEnumerable<ProductsByRefTable> productsNeedDelayTimeToSumarizeDataInCache;

        public ProductServiceImplTests()
        {
            Func<int, bool> canViewProduct = (productId) => true;
            Func<int, string> getProductName = (productId) => $"Product {productId}";
            cacheService = Substitute.For<ICacheService>();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            productServiceImpl = new ProductServiceImpl(dynamicRepository, cacheService, canViewProduct, getProductName);

            dynamicRepository.Fetch<ProductsByRefTable>(Arg.Any<GetProductsByRefTableCriteria>()).Returns(new List<ProductsByRefTable>
            {
                new ProductsByRefTable(1, 2),
                new ProductsByRefTable(2, 2)
            });

            IEnumerable<ProductsByRefTable> productsNeedDelayTimeToSumarizeData;

            cacheService.TryGet(ProductsNeedDelayTimeToSumarizeDataCacheKey, out productsNeedDelayTimeToSumarizeData).Returns(x =>
            {
                x[1] = productsNeedDelayTimeToSumarizeDataInCache;

                return productsNeedDelayTimeToSumarizeDataInCache != null;
            });

            cacheService.Set(ProductsNeedDelayTimeToSumarizeDataCacheKey, productsNeedDelayTimeToSumarizeDataInCache);

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
            productServiceImpl = new ProductServiceImpl(dynamicRepository, cacheService, null, null);

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
            cacheService.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);
            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
            });

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
            cacheService.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);
            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
            });

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
            cacheService.TryGet($"{ProductCacheKey}-{custId}", out cachedProducts).Returns(false);
            dynamicRepository.Fetch<Product>(Arg.Any<GetProductsCriteria>()).Returns(allCustomerProductIds);
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
            });

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
            dynamicRepository.Fetch<Product>(Arg.Any<ICriteria>()).Returns(customerProducts);
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<Product>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<Product>>>(1);
                return func();
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<ProductsByRefTable>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<ProductsByRefTable>>>(1);
                return func();
            });

            // Act
            var actualProducts = productServiceImpl.GetProductsNeedDelayTimeToSumarizeDataByCustomer(1, string.Empty, 1);

            // Assert
            Assert.Equal(expectProducts, actualProducts);
        }       

        [Fact]
        public void CanViewBySite_NonOfferedProduct_ReturnFalse()
        {
            // Arrange
            var site = "IBCINdo";
            var products = new List<ProductBySiteModel>() { new ProductBySiteModel(22), new ProductBySiteModel(23) };
            IEnumerable<ProductBySiteModel> cachedProducts;
            cacheService.TryGet($"Alpha.Reports.Services.Impl.ProductsBySite-{site}", out cachedProducts).Returns(false);

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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<ProductBySiteModel>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<ProductBySiteModel>>>(1);
                return func();
            });
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
            Assert.Null(act.LeagueGroupId);

            // Assert
            Assert.Empty(act.LeagueGroupIds);

            // Assert
            Assert.Null(act.TicketType);

            // Assert
            Assert.Empty(act.TicketTypeIds);

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
        public void StatusModel_ParamCtor_AlwaysReturnsCorrectProperties()
        {
            // Arrange
            var act = new StatusModel(0, null);

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