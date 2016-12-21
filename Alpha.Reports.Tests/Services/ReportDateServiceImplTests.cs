namespace Alpha.Reports.Tests.Services
{
    using System;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Reports.Criteria;
    using Reports.Models;
    using Reports.Services;
    using Reports.Services.Impl;
    using Xunit;

    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private readonly IDynamicRepository repository;
        private readonly ReportDateService service;
        private readonly DateTime balanceUpDay;
        private readonly DateTime moveBetsDay;
        private readonly Cache cacheForTesting;

        public ReportDateServiceImplTests()
        {
            cacheForTesting = Substitute.For<Cache>();
            CacheService.SwitchCacheStorage(cacheForTesting);
            CacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);

            repository = Substitute.For<IDynamicRepository>();
            service = new ReportDateServiceImpl(repository);
            balanceUpDay = new DateTime(2000, 1, 1);
            moveBetsDay = new DateTime(2010, 1, 1);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataInCache_ReturnsDataGetFromCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);

            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            cacheForTesting.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return true;
            });

            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.True(balanceUpDay == actual.BalanceUpDay && moveBetsDay == actual.MoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_NoDataInCache_DynamicRepositoryGetWasCalled()
        {
            // Arrange
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>())
                .Returns(new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay));

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            repository.Received().Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>());
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_DataReturnedFromRepository_WasAddedToCache()
        {
            // Arrange
            var reportDates = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria())
                .ReturnsForAnyArgs(reportDates);

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            cacheForTesting.Received().Set(
                BalanceUpDayAndMoveBetsDayCacheKey,
                Arg.Is<BalanceUpDayAndMoveBetsDay>(data => data.BalanceUpDay == reportDates.BalanceUpDay && data.MoveBetsDay == reportDates.MoveBetsDay),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);

            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            cacheForTesting.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return true;
            });

            CacheService.SwitchCacheStorage(cacheForTesting);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache.MoveBetsDay, actual);
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);

            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            cacheForTesting.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return true;
            });

            CacheService.SwitchCacheStorage(cacheForTesting);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInCache.BalanceUpDay, actual);
        }
    }
}
