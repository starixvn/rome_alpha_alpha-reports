namespace Alpha.Reports.Tests.Services
{
    using System;
    using Constants;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Fixtures;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Services;
    using Xunit;

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
            DateTimeManager.Now = DefaultVariables.CurrentDate;

            ConfiguredCacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);

            repository = Substitute.For<IDynamicRepository>();
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>())
                .Returns(new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay));

            service = new ReportDateServiceImpl(repository);
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
        public void ValidateDateRange_HistoryReportAndFromDateLessThan3Years_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate.AddYears(-4),
                toDate: DateTime.Now,
                isHistoricReport: true);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_HistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddDays(-1),
                isHistoricReport: true);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public void ValidateDateRange_HistoryReportAndCrossMonthRange_ReturnsFalse(int monthDiff)
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddMonths(monthDiff),
                isHistoricReport: true);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_HistoryReportAndValidRange_ReturnsTrue()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate,
                isHistoricReport: true);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndFromDateLessThan3Months_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate.AddMonths(-4),
                toDate: DateTime.Now,
                isHistoricReport: false);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddDays(-1),
                isHistoricReport: false);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndValidDateRange_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate,
                isHistoricReport: false);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateDateRange_DateRageTypeIsNotSupported_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange((DateRangeType)10, DateTime.Now, DateTime.Now);

            // Act
            var isValid = service.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            DateRange dateRange = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.ValidateDateRange(dateRange));
        }

        [Fact]
        public void GetMinDate_ThreeMonthCurrentOrThreeYearHistory_HistoricReport_ReturnFirstDateOfPrevious3Years()
        {
            // Act
            var actualMinDate = service.GetMinDate(DateRangeType.ThreeMonthCurrentOrThreeYearHistory, isHistoricReport: true);

            // Assert
            Assert.Equal(DefaultVariables.CurrentDate.AddYears(-3).FirstDayOfMonth(), actualMinDate);
        }

        [Fact]
        public void GetMinDate_ThreeMonthCurrentOrThreeYearHistory_CurrentReport_ReturnFirstDateOfPrevious3Months()
        {
            // Act
            var actualMinDate = service.GetMinDate(DateRangeType.ThreeMonthCurrentOrThreeYearHistory, isHistoricReport: false);

            // Assert
            Assert.Equal(DefaultVariables.CurrentDate.AddMonths(-3).FirstDayOfMonth(), actualMinDate);
        }

        [Fact]
        public void GetMinDate_DateRangeTypeIsUnknown_ReturnCurrentDate()
        {
            // Act
            var actualMinDate = service.GetMinDate((DateRangeType)10, isHistoricReport: false);

            // Assert
            Assert.Equal(DefaultVariables.CurrentDate, actualMinDate);
        }

        [Fact]
        public void Init_Always_SetupReportDateRangeTypes()
        {
            // Act
            ReportDateServiceImpl.RegisterReportDateRangeTypes();

            // Assert
            Assert.True(
                WinLossServiceImpl.DateRangeType == DateRangeType.ThreeMonthCurrentOrThreeYearHistory
                && WinLossDetailServiceImpl.DateRangeType == DateRangeType.ThreeMonthCurrentOrThreeYearHistory);
        }
    }
}