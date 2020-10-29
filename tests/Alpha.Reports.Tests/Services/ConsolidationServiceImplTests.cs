namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.DataAccesses;
    using Constants;
    using Fanex.Data.Repository;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.Consolidation;
    using Reports.Models;
    using Reports.Models.Consolidation;
    using Reports.Params.Consolidation;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("AnyValueCreatorCollection")]
    public class ConsolidationServiceImplTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ConsolidationServiceImpl consolidationServiceImpl;
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossDetailDataAccess winLossDetailDataAccess;
        private readonly ProductService productService;

        public ConsolidationServiceImplTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;

            dynamicRepository = Substitute.For<IDynamicRepository>();
            var minDate = DefaultVariablesTest.CurrentDate;
            var data = new List<ConsolidationProduct>
            {
               new ConsolidationProduct(id: 1, name: null, isEnable: true, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 2, name: null, isEnable: true, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 3, name: null, isEnable: false, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 4, name: null, isEnable: false, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 5, name: null, isEnable: true, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 6, name: null, isEnable: false, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 7, name: null, isEnable: true, endOfSupportDate: minDate.AddDays(-1), type: 0, orderNo: 0),
               new ConsolidationProduct(id: 8, name: null, isEnable: false, endOfSupportDate: minDate, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 9, name: null, isEnable: false, endOfSupportDate: minDate.AddDays(-1), type: 0, orderNo: 0),
               new ConsolidationProduct(id: 10, name: null, isEnable: false, endOfSupportDate: null, type: 0, orderNo: 0),
               new ConsolidationProduct(id: 11, name: null, isEnable: true, endOfSupportDate: null, type: 0, orderNo: 0)
           };

            dynamicRepository.Fetch<ConsolidationProduct>(Arg.Any<GetConsolidationReportProductsCriteria>()).Returns(data);

            var reportDateService = Substitute.For<ReportDateService>();
            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetProductName(Arg.Any<int>()).Returns("ProductName");

            winLossDetailDataAccess = Substitute.For<WinLossDetailDataAccess>();
            productService = Substitute.For<ProductService>();
            consolidationServiceImpl = new ConsolidationServiceImpl(dynamicRepository, reportDateService, cacheRefService, winLossDetailDataAccess, productService);

            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetCurrencies_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var data = new List<CurrencyModel>
            {
                new CurrencyModel(id: anyValueCreatorFixture.Create<byte>(), name: anyValueCreatorFixture.Create<string>())
            };

            dynamicRepository.Fetch<CurrencyModel>(Arg.Any<GetCurrencyCriteria>())
                .Returns(data);
            var param = new GetCurrencyParam(custId: anyValueCreatorFixture.Create<int>());
            var expectedCurrencies = data;

            // Act
            var actualCurrencies = consolidationServiceImpl.GetCurrencies(param);

            // Assert
            Assert.Equal(expectedCurrencies, actualCurrencies);
        }

        [Fact]
        public void GetProducts_Always_ReturnsEnableProducts()
        {
            // Arrange
            var minDate = DefaultVariablesTest.CurrentDate;
            var param = new GetConslidationReportProductsParam(custId: anyValueCreatorFixture.Create<int>(), minDate: minDate);
            var expectedProducts = new List<Product>
            {
                new Product(1, null, minDate, 0, 0, cacheRefService.GetProductName),
                new Product(2, null, minDate, 0, 0, cacheRefService.GetProductName),
                new Product(5, null, minDate, 0, 0, cacheRefService.GetProductName),
                new Product(11, null, null, 0, 0, cacheRefService.GetProductName)
            };

            // Act
            var actualProducts = consolidationServiceImpl.GetProducts(param);

            // Assert
            Assert.True(compareLogic.Compare(expectedProducts, actualProducts.ToList()).AreEqual);
        }

        [Fact]
        public void GetConsolidationByUsername_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var data = new List<ConsolidationByUsername>
            {
                anyValueCreatorFixture.Create<ConsolidationByUsername>(),
                anyValueCreatorFixture.Create<ConsolidationByUsername>(),
                anyValueCreatorFixture.Create<ConsolidationByUsername>()
            };

            dynamicRepository.Fetch<ConsolidationByUsername>(Arg.Any<GetConsolidationCriteria>()).Returns(data);
            var param = anyValueCreatorFixture.Create<GetConsolidationParam>();
            var expectedData = data;

            // Act
            var actualData = consolidationServiceImpl.GetConsolidationByUsername(param);

            // Assert
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetConsolidationByDate_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var data = new List<ConsolidationByDate>
            {
                anyValueCreatorFixture.Create<ConsolidationByDate>(),
                anyValueCreatorFixture.Create<ConsolidationByDate>(),
                anyValueCreatorFixture.Create<ConsolidationByDate>()
            };

            dynamicRepository.Fetch<ConsolidationByDate>(Arg.Any<GetConsolidationCriteria>()).Returns(data);
            var param = anyValueCreatorFixture.Create<GetConsolidationParam>();
            var expectedData = data;

            // Act
            var actualData = consolidationServiceImpl.GetConsolidationByDate(param);

            // Assert
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetConsolidationByProduct_Always_ReturnsDataFromDynamicRepository()
        {
            // Arrange
            var data = new List<ConsolidationByProduct>
            {
                anyValueCreatorFixture.Create<ConsolidationByProduct>(),
                anyValueCreatorFixture.Create<ConsolidationByProduct>(),
                anyValueCreatorFixture.Create<ConsolidationByProduct>()
            };

            dynamicRepository.Fetch<ConsolidationByProduct>(Arg.Any<GetConsolidationCriteria>()).Returns(data);
            var param = anyValueCreatorFixture.Create<GetConsolidationParam>();
            var expectedData = data;

            // Act
            var actualData = consolidationServiceImpl.GetConsolidationByProduct(param);

            // Assert
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public void FilterProductIdsByCustomer_NullProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            IEnumerable<int> productIds = null;
            var expectedProductIds = new List<int> { 1, 2, 5, 11 };

            // Act
            var actualProductIds = consolidationServiceImpl.FilterProductIdsByCustomer(0, productIds, DefaultVariablesTest.CurrentDate);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void FilterProductIdsByCustomer_EmptyProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            var productIds = Enumerable.Empty<int>();
            var expectedProductIds = new List<int> { 1, 2, 5, 11 };

            // Act
            var actualProductIds = consolidationServiceImpl.FilterProductIdsByCustomer(0, productIds, DefaultVariablesTest.CurrentDate);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void FilterProductIdsByCustomer_HasProductIds_ReturnsCorrectProductIds()
        {
            // Arrange
            var productIds = new List<int> { 1, 3, 5 };
            var expectedProductIds = new List<int> { 1, 5 };

            // Act
            var actualProductIds = consolidationServiceImpl.FilterProductIdsByCustomer(0, productIds, DefaultVariablesTest.CurrentDate);

            // Assert
            Assert.Equal(expectedProductIds, actualProductIds);
        }

        [Fact]
        public void GetConsolidationExcludeProduct_EmptyExcludeProducts_ReturnsCorrectValue()
        {
            // Arrange
            var minDate = DefaultVariablesTest.CurrentDate;
            IEnumerable<Product> products = new List<Product>
            {
                new Product(id: 1, name: null, endOfSupportDate: minDate, type: 0, orderNo: 0, getProductDisplayName: cacheRefService.GetProductName),
                new Product(id: 2, name: null, endOfSupportDate: minDate, type: 0, orderNo: 0, getProductDisplayName: cacheRefService.GetProductName),
                new Product(id: 5, name: null, endOfSupportDate: minDate, type: 0, orderNo: 0, getProductDisplayName: cacheRefService.GetProductName),
                new Product(id: 11, name: null, endOfSupportDate: null, type: 0, orderNo: 0, getProductDisplayName: cacheRefService.GetProductName)
            };

            var expectedValue = new ConsolidationExcludeProduct(products);

            // Act
            var actualValue = consolidationServiceImpl.GetConsolidationExcludeProduct(0, DefaultVariablesTest.CurrentDate);

            // Assert
            Assert.True(compareLogic.Compare(expectedValue, actualValue).AreEqual);
        }
    }
}
