namespace Alpha.Reports.Tests.Services.WinLoss
{
    using System;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Params;
    using Reports.Services;
    using Reports.Services.Impl;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossServiceImplTests
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";
        private readonly ReportDateService reportDateService;
        private readonly ProductService productService;
        private readonly WinLossDataAccess dataAccess;
        private readonly WinLossServiceImpl winLossServiceImpl;
        private readonly IDynamicRepository dynamicRepository;
        private readonly Cache cacheForTesting;
        private CompareLogic _compare;

        public WinLossServiceImplTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            reportDateService = Substitute.For<ReportDateService>();
            productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDataAccess>();
            winLossServiceImpl = new WinLossServiceImpl(reportDateService, productService, dataAccess, dynamicRepository);

            cacheForTesting = Substitute.For<Cache>();
            CacheService.SwitchCacheStorage(cacheForTesting);
            _compare = new CompareLogic();
        }

        [Fact]
        public void GetWinLoss_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossParam nullParam = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossServiceImpl.GetWinLoss(nullParam));
        }

        [Fact]
        public void GetWinLoss_InvalidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                DateTime.Now.AddYears(-4),
                DateTime.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            var expectedData = Enumerable.Empty<WinLoss>();

            // Act
            var actualData = winLossServiceImpl.GetWinLoss(param);
            var result = _compare.Compare(expectedData, actualData);

            // Assert
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                DateTime.Now,
                DateTime.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            var expectedData = Enumerable.Empty<WinLoss>();

            // Act
            dataAccess.GetWinLoss(Arg.Any<WinLossCriteria>()).Returns(expectedData);
            var actualData = winLossServiceImpl.GetWinLoss(param);
            var result = _compare.Compare(expectedData, actualData);

            // Assert
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_CallGetWinLossDataAccess()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                DateTime.Now,
                DateTime.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            // Act
            winLossServiceImpl.GetWinLoss(param);

            // Assert
            dataAccess.Received().GetWinLoss(Arg.Any<WinLossCriteria>());
        }

        ////[Fact]
        ////public void GetEnteredSportResults_IsValid_ReturnsDefaultReportData()
        ////{
        ////    // Arrange
        ////    var expectedData = Enumerable.Empty<EnteredSportResult>();

        ////    var cachedEnteredSportResult = Enumerable.Empty<EnteredSportResult>();
        ////    cacheForTesting.TryGet(EnteredResultsCacheKey, out cachedEnteredSportResult).Returns(false);
        ////    dynamicRepository.Fetch<EnteredSportResult>(Arg.Any<EnteredSportDailyCriteria>()).Returns(expectedData);

        ////    // Act
        ////    var actualData = winLossServiceImpl.GetEnteredSportResults();
        ////    var result = _compare.Compare(expectedData, actualData);

        ////    // Assert
        ////    Assert.True(result.AreEqual);
        ////}

        ////[Theory]
        ////[ClassData(typeof(WinLossTestCases))]
        ////public void GetWinLoss_ValidParam_ReturnsDefaultReportData()
        ////{
        ////    // Arrange
        ////    WinLossParam param = new WinLossParam(
        ////        0,
        ////        DateTime.Now,
        ////        DateTime.Now,
        ////        Enumerable.Empty<int>(),
        ////        Enumeration.FromValue<UserLevel>(4),
        ////        false);

        ////    var grantTotal = new WinLoss();
        ////    var winLossServiceImplSubstitute = Substitute.ForPartsOf<WinLossServiceImpl>(reportDateService, productService, dataAccess);

        ////    winLossServiceImplSubstitute.BuildGrandTotal(superWinLoss, UserLevel.Super).Returns(grantTotal);
        ////    dataAccess.GetWinLosses(Arg.Any<WinLossCriteria>()).Returns(superWinLoss);
        ////    var expectedReportData = new ReportData<WinLoss>(superWinLoss, 0, grantTotal);

        ////    var expectedReportData = new ReportData<WinLoss>(Enumerable.Empty<WinLoss>(), 0, new WinLoss());

        ////    // Act
        ////    var actualReportData = winLossServiceImpl.GetWinLoss(param);

        ////    // Assert
        ////    Assert.True(WinLossReportDataComparer.Equals(expectedReportData, actualReportData));
        ////}

        ////[Theory]
        ////[ClassData(typeof(WinLossTestCases))]
        ////public void BuildGrandTotal_ValidParam_ReturnsCorrectGrandTotalWinLoss(
        ////    List<WinLoss> winLosses, 
        ////    UserLevel userLevel,
        ////    WinLoss expectedGrantTotal)
        ////{
        ////    // Arrange

        ////    // Act
        ////    var actualGrantTotal = winLossServiceImpl.BuildGrandTotal(winLosses, userLevel);

        ////    // Assert
        ////    Assert.True(WinLossComparer.Equals(expectedGrantTotal, actualGrantTotal));
        ////}
    }
}
