namespace Alpha.Reports.Tests.Services.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Services;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Params;
    using Reports.Services;
    using TestCases;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossServiceImplTests
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";

        private readonly WinLossDataAccess dataAccess;
        private readonly WinLossServiceImpl winLossServiceImpl;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly CacheRefService cacheRefService;
        private readonly ProductService productService;
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly ReportDateService reportDateService;
        private readonly IPartialUMService partialUMService;

        public WinLossServiceImplTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            cacheService = Substitute.For<ICacheService>();
            productService = Substitute.For<ProductService>();
            dataAccess = Substitute.For<WinLossDataAccess>();
            cacheRefService = Substitute.For<CacheRefService>();
            reportDateService = Substitute.For<ReportDateService>();
            partialUMService = Substitute.For<IPartialUMService>();
            winLossServiceImpl = new WinLossServiceImpl(reportDateService, productService, dataAccess, dynamicRepository, cacheRefService, cacheService, partialUMService);
            compareLogic = new CompareLogic();
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
        public void GetWinLoss_ValidParam_ReturnsDefaultReportData()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                1,
                "",
                1,
                currentDate,
                currentDate,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                false,
                string.Empty,
                0);

            var expectedData = Enumerable.Empty<WinLossModel>();

            // Act
            dataAccess.GetWinLoss(Arg.Any<WinLossMemberCriteria>()).Returns(expectedData);
            var actualData = winLossServiceImpl.GetWinLoss(param);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_CallGetWinLossDataAccess()
        {
            // Arrange
            WinLossParam param = new WinLossParam(
                0,
                1,
                "",
                1,
                currentDate,
                currentDate,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                false,
                string.Empty,
                0);

            // Act
            winLossServiceImpl.GetWinLoss(param);

            // Assert
            dataAccess.Received().GetWinLoss(Arg.Any<WinLossMemberCriteria>());
        }

        [Fact]
        public void GetWinLoss_UserIsInLimitedDateRange_UseLimitedDate()
        {
            // Arrange
            var limitedDate = currentDate.AddDays(1);
            var param = new WinLossParam(
                0,
                1,
                "",
                1,
                currentDate,
                currentDate.AddDays(1),
                limitedDate,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                false,
                string.Empty,
                0);

            // Act
            winLossServiceImpl.GetWinLoss(param);

            // Assert
            dataAccess.Received().GetWinLoss(Arg.Is<WinLossMemberCriteria>(i => i.FromDate == limitedDate));
        }

        [Fact]
        public void GetEnteredSportResults_IsValid_ReturnsDefaultReportData()
        {
            // Arrange
            var dataEnteredSport = Enumerable.Empty<DailyEnteredSport>();
            dynamicRepository.Fetch<DailyEnteredSport>(Arg.Any<DailyEnteredSportCriteria>()).Returns(dataEnteredSport);
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<IEnumerable<EnteredSportResult>>>(), Arg.Any<CacheItemOptions>()).Returns(x =>
            {
                var func = x.ArgAt<Func<IEnumerable<EnteredSportResult>>>(1);
                return func();
            });

            // Act
            var actualData = winLossServiceImpl.GetEnteredSportResults();

            // Assert
            var expectedData = Enumerable.Empty<EnteredSportResult>();
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [MemberData("EnteredResultsSources", MemberType = typeof(EnteredResultsTestCases))]
        public void GetDataEnteredSportResults_IsValid_ReturnsReportData(
            IEnumerable<DailyEnteredSport> dailyEnteredSportData,
            IEnumerable<EnteredSportResult> expectedEnteredSportResultData)
        {
            // Arrange
            dynamicRepository.Fetch<DailyEnteredSport>(Arg.Any<DailyEnteredSportCriteria>()).Returns(dailyEnteredSportData);
            cacheRefService.GetSportName(1).Returns("Soccer");
            cacheRefService.GetSportName(2).Returns("BasketBall");

            // Act
            var actualEnteredSportResultData = winLossServiceImpl.GetDataEnteredSportResults();

            // Assert
            var result = compareLogic.Compare(expectedEnteredSportResultData.ToList(), actualEnteredSportResultData.ToList());
            Assert.True(result.AreEqual);
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
            var result = compareLogic.Compare(expectedEnteredSportResultData, actualEnteredSportResultData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetDataEnteredSportResults_DefaultCtor_ReturnsCorrectProperties()
        {
            // Arrange
            var model = new EnteredSportResult();

            // Assert
            Assert.True(model.SportTypeId == 0);

            // Assert
            Assert.Null(model.SportName);

            // Assert
            Assert.Null(model.EnteredSportToday);

            // Assert
            Assert.Null(model.EnteredSportYesterday);
        }
    }
}