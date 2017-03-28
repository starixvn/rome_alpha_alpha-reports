namespace Alpha.Reports.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Services;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    [Collection("FanexCacheCollection")]
    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private readonly IDynamicRepository repository;
        private readonly ReportDateServiceImpl service;
        private readonly DateTime balanceUpDay;
        private readonly DateTime moveBetsDay;
        private readonly FanexCacheFixture fanexCacheFixture;
        private BalanceUpDayAndMoveBetsDay dataInCache;

        public ReportDateServiceImplTests(FanexCacheFixture fanexCacheFixture)
        {
            this.fanexCacheFixture = fanexCacheFixture;

            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            balanceUpDay = new DateTime(2000, 1, 1);
            moveBetsDay = new DateTime(2010, 1, 1);
            dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            fanexCacheFixture.CacheForTesting.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return dataInCache != null;
            });
            SystemTime.Today = DefaultVariables.CurrentDate;

            ConfiguredCacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);

            repository = Substitute.For<IDynamicRepository>();
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>())
                .Returns(new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay));

            service = Substitute.For<ReportDateServiceImpl>(repository);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataInCache_ReturnsDataGetFromCache()
        {
            // Arrange
            ConfiguredCacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.True(balanceUpDay == actual.BalanceUpDay && moveBetsDay == actual.MoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_NoDataInCache_DynamicRepositoryGetWasCalled()
        {
            // Arrange
            dataInCache = null;

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            repository.Received().Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>());
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataReturnedFromRepository_WasAddedToCache()
        {
            // Arrange
            dataInCache = null;

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            fanexCacheFixture.CacheForTesting.Received().Set(
                Arg.Is(BalanceUpDayAndMoveBetsDayCacheKey),
                Arg.Is<BalanceUpDayAndMoveBetsDay>(data => data.BalanceUpDay == balanceUpDay && data.MoveBetsDay == moveBetsDay),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            ConfiguredCacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache.MoveBetsDay, actual);
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            ConfiguredCacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInCache.BalanceUpDay, actual);
        }

        [Fact]
        public void GetTimeRanges_BothLessThanBalanceUpDay_ReturnFromToDate()
        {
            // Arrange
            var fromDate = balanceUpDay.AddDays(-1);
            var toDate = balanceUpDay.AddDays(-1);
            var expectedTimeRange = new List<TimeRange> { new TimeRange(fromDate, toDate) };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetTimeRanges_FromDateLessThanBalanceUpdateAndToDateNotGreaterThanMoveBetsDate_ReturnCorrectRanges()
        {
            // Arrange
            var fromDate = balanceUpDay.AddDays(-1);
            var toDate = moveBetsDay;
            var expectedTimeRange = new List<TimeRange>
            {
                new TimeRange(fromDate, balanceUpDay.AddDays(-1)),
                new TimeRange(balanceUpDay, toDate)
            };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetTimeRanges_FromDateLessThanBalanceUpdateAndToDateGreaterThanMoveBetsDate_ReturnCorrectRanges()
        {
            // Arrange
            var fromDate = balanceUpDay.AddDays(-1);
            var toDate = moveBetsDay.AddDays(1);
            var expectedTimeRange = new List<TimeRange>
            {
                new TimeRange(fromDate, balanceUpDay.AddDays(-1)),
                new TimeRange(balanceUpDay, moveBetsDay),
                new TimeRange(moveBetsDay.AddDays(1), toDate),
            };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetTimeRanges_FromDateNotGreaterThanMoveBetsDateAndToDateNotGreaterThanMoveBetsDate_ReturnCorrectRanges()
        {
            // Arrange
            var fromDate = balanceUpDay;
            var toDate = moveBetsDay;
            var expectedTimeRange = new List<TimeRange> { new TimeRange(fromDate, toDate) };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetTimeRanges_FromDateNotGreaterThanMoveBetsDateAndToDateGreaterThanMoveBetsDate_ReturnCorrectRanges()
        {
            // Arrange
            var fromDate = balanceUpDay;
            var toDate = moveBetsDay.AddDays(1);
            var expectedTimeRange = new List<TimeRange>
            {
                new TimeRange(fromDate, moveBetsDay),
                new TimeRange(moveBetsDay.AddDays(1), toDate)
            };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetTimeRanges_FromDateGreaterThanMoveBetsDateAndToDateGreaterThanMoveBetsDate_ReturnCorrectRanges()
        {
            // Arrange
            var fromDate = moveBetsDay.AddDays(1);
            var toDate = moveBetsDay.AddDays(1);
            var expectedTimeRange = new List<TimeRange> { new TimeRange(fromDate, toDate) };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }
    }
}