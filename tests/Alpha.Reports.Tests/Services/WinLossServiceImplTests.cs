namespace Alpha.Reports.Tests.Services.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Constants;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Params;
    using Reports.Services;
    using Reports.Services.Impl;
    using TestCases;
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
        private readonly CacheRefService cacheRefService;

        public WinLossServiceImplTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            reportDateService = Substitute.For<ReportDateService>();
            productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDataAccess>();
            cacheRefService = Substitute.For<CacheRefService>();
            winLossServiceImpl = new WinLossServiceImpl(reportDateService, productService, dataAccess, dynamicRepository, cacheRefService);

            cacheForTesting = Substitute.For<Cache>();
            CacheService.SwitchCacheStorage(cacheForTesting);
            DateTimeManager.Now = DefaultVariables.CurrentDate;
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
                DateTimeManager.Now.AddYears(-4),
                DateTimeManager.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            var expectedData = Enumerable.Empty<WinLoss>();

            // Act
            var actualData = winLossServiceImpl.GetWinLoss(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetWinLoss_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                DateTimeManager.Now,
                DateTimeManager.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            var expectedData = Enumerable.Empty<WinLoss>();

            // Act
            dataAccess.GetWinLoss(Arg.Any<WinLossCriteria>()).Returns(expectedData);
            var actualData = winLossServiceImpl.GetWinLoss(param);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetWinLoss_CallGetWinLossDataAccess()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                DateTimeManager.Now,
                DateTimeManager.Now,
                Enumerable.Empty<int>(),
                Enumeration.FromValue<UserLevel>(4),
                false);

            // Act
            winLossServiceImpl.GetWinLoss(param);

            // Assert
            dataAccess.Received().GetWinLoss(Arg.Any<WinLossCriteria>());
        }

        [Fact]
        public void GetEnteredSportResults_IsValid_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedData = Enumerable.Empty<EnteredSportResult>();

            IEnumerable<EnteredSportResult> cachedEnteredSportResult;
            cacheForTesting.TryGet(EnteredResultsCacheKey, out cachedEnteredSportResult).Returns(false);

            var dataEnteredSport = Enumerable.Empty<DailyEnteredSport>();
            dynamicRepository.Fetch<DailyEnteredSport>(Arg.Any<DailyEnteredSportCriteria>()).Returns(dataEnteredSport);

            // Act
            var actualData = winLossServiceImpl.GetEnteredSportResults();

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Theory]
        [MemberData("EnteredResultsSources", MemberType = typeof(EnteredResultsTestCases))]
        public void GetDataEnteredSportResults_IsValid_ReturnsReportData(
            List<DailyEnteredSport> dailyEnteredSportData,
            List<EnteredSportResult> expectedEnteredSportResultData)
        {
            // Arrange
            dynamicRepository.Fetch<DailyEnteredSport>(Arg.Any<DailyEnteredSportCriteria>()).Returns(dailyEnteredSportData);
            cacheRefService.GetSportName(1).Returns("Soccer");
            cacheRefService.GetSportName(2).Returns("BasketBall");

            // Act
            var actualEnteredSportResultData = winLossServiceImpl.GetEnteredSportResults();

            // Assert
            AssertExtension.Equal(expectedEnteredSportResultData, actualEnteredSportResultData);
        }

        [Fact]
        public void GetDataEnteredSportResults_DailyEnteredSportEmpty_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedEnteredSportResultData = Enumerable.Empty<EnteredSportResult>();
            var dailyEnteredSportData = Enumerable.Empty<DailyEnteredSport>();
            dynamicRepository.Fetch<DailyEnteredSport>(Arg.Any<DailyEnteredSportCriteria>()).Returns(dailyEnteredSportData);

            // Act
            var actualEnteredSportResultData = winLossServiceImpl.GetEnteredSportResults();

            // Assert
            AssertExtension.Equal(expectedEnteredSportResultData, actualEnteredSportResultData);
        }
    }
}
