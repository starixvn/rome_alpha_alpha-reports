namespace Alpha.Reports.Tests.Services
{
    using Constants;
    using Extensions;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Services;
    using Reports.Utils;
    using System;
    using System.Collections.Generic;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private readonly IDynamicRepository repository;
        private readonly ReportDateServiceImpl service;
        private readonly DateTime balanceUpDay;
        private readonly DateTime moveBetsDay;
        private readonly ICacheService cacheService;
        private BalanceUpDayAndMoveBetsDay dataInCache;

        public ReportDateServiceImplTests()
        {
            cacheService = Substitute.For<ICacheService>();
            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            balanceUpDay = new DateTime(2010, 1, 1);
            moveBetsDay = new DateTime(2010, 1, 5);
            dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            cacheService.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return dataInCache != null;
            });
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            cacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);

            repository = Substitute.For<IDynamicRepository>();
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>())
                .Returns(new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay));

            service = Substitute.For<ReportDateServiceImpl>(repository, cacheService);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataInCache_ReturnsDataGetFromCache()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);

            // Act
            var actual = service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.True(balanceUpDay == actual.BalanceUpDay && moveBetsDay == actual.MoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataReturnedFromRepository_WasAddedToCache()
        {
            // Arrange
            dataInCache = null;

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            cacheService.Received().GetOrSet(
                Arg.Is(BalanceUpDayAndMoveBetsDayCacheKey),
                Arg.Is<Func<BalanceUpDayAndMoveBetsDay>>((x) => x.Invoke().BalanceUpDay == balanceUpDay && x.Invoke().MoveBetsDay == moveBetsDay),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);

            // Act
            var actual = service.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache.MoveBetsDay, actual);
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInCache.BalanceUpDay, actual);
        }

        [Fact]
        public void GetTimeRanges_BothLessThanBalanceUpDay_ReturnFromToDate()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
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
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
            var fromDate = moveBetsDay.AddDays(1);
            var toDate = moveBetsDay.AddDays(1);
            var expectedTimeRange = new List<TimeRange> { new TimeRange(fromDate, toDate) };

            // Act
            var actualTimeRanges = service.GetTimeRangesToGetReportDataBetweenTables(fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectedTimeRange, actualTimeRanges);
        }

        [Fact]
        public void GetStoreMappingSuffix_GreaterThanMoveBetsDay_ReturnsEmpty()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
            var expectedSuffix = string.Empty;
            var fromDate = new DateTime(2010, 1, 6);

            // Act
            var actualSuffix = service.GetStoreMappingSuffix(fromDate);

            // Assert
            Assert.Equal(expectedSuffix, actualSuffix);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(1)]
        public void GetStoreMappingSuffix_LessThanMoveBetsDayAndNotLessThanBalanceUpDay_Returns_14(int date)
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
            var expectedSuffix = "_14";
            var fromDate = new DateTime(2010, 1, date);

            // Act
            var actualSuffix = service.GetStoreMappingSuffix(fromDate);

            // Assert
            Assert.Equal(expectedSuffix, actualSuffix);
        }

        [Fact]
        public void GetStoreMappingSuffix_LessThanBalanceUpDay_Returns_WH()
        {
            // Arrange
            cacheService.GetOrSet(Arg.Any<string>(), Arg.Any<Func<BalanceUpDayAndMoveBetsDay>>(), Arg.Any<CacheItemOptions>())
                .Returns(dataInCache);
            var expectedSuffix = "_WH";
            var fromDate = new DateTime(2009, 12, 1);

            // Act
            var actualSuffix = service.GetStoreMappingSuffix(fromDate);

            // Assert
            Assert.Equal(expectedSuffix, actualSuffix);
        }
    }
}