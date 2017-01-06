namespace Alpha.Reports.Tests.Services.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Params;
    using Reports.Services;
    using TestCases;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("FanexCacheCollection")]
    public class WinLossServiceImplTests
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";

        private readonly WinLossDataAccess dataAccess;
        private readonly WinLossServiceImpl winLossServiceImpl;
        private readonly IDynamicRepository dynamicRepository;
        private readonly FanexCacheFixture fanexCacheFixture;
        private readonly CacheRefService cacheRefService;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossServiceImplTests(FanexCacheFixture fanexCacheFixture)
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            var reportDateService = Substitute.For<ReportDateService>();
            var productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDataAccess>();
            cacheRefService = Substitute.For<CacheRefService>();
            winLossServiceImpl = new WinLossServiceImpl(reportDateService, productService, dataAccess, dynamicRepository, cacheRefService);

            this.fanexCacheFixture = fanexCacheFixture;
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
                currentDate.AddYears(-4),
                currentDate,
                Enumerable.Empty<int>(),
                UserLevel.Super,
                false);

            var expectedData = Enumerable.Empty<WinLossModel>();

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
                currentDate,
                currentDate,
                Enumerable.Empty<int>(),
                UserLevel.Super,
                false);

            var expectedData = Enumerable.Empty<WinLossModel>();

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
                currentDate,
                currentDate,
                Enumerable.Empty<int>(),
                UserLevel.Super,
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
            fanexCacheFixture.CacheForTesting.TryGet(EnteredResultsCacheKey, out cachedEnteredSportResult).Returns(false);

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
            var actualEnteredSportResultData = winLossServiceImpl.GetDataEnteredSportResults();

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
            var actualEnteredSportResultData = winLossServiceImpl.GetDataEnteredSportResults();

            // Assert
            AssertExtension.Equal(expectedEnteredSportResultData, actualEnteredSportResultData);
        }
    }
}