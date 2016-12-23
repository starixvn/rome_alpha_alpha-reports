namespace Alpha.Reports.Tests.Services
{
    using System;
    using Fanex.Caching;
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
        private BalanceUpDayAndMoveBetsDay dataInCache;

        public ReportDateServiceImplTests()
        {
            cacheForTesting = Substitute.For<Cache>();
            BalanceUpDayAndMoveBetsDay balanceUpDayAndMoveBetsDay;
            balanceUpDay = new DateTime(2000, 1, 1);
            moveBetsDay = new DateTime(2010, 1, 1);
            dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            cacheForTesting.TryGet(BalanceUpDayAndMoveBetsDayCacheKey, out balanceUpDayAndMoveBetsDay).Returns(x =>
            {
                x[1] = dataInCache;

                return dataInCache != null;
            });

            CacheService.SwitchCacheStorage(cacheForTesting);
            CacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);

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
            cacheForTesting.Received().Set(
                BalanceUpDayAndMoveBetsDayCacheKey,
                Arg.Is<BalanceUpDayAndMoveBetsDay>(data => data.BalanceUpDay == balanceUpDay && data.MoveBetsDay == moveBetsDay),
                Arg.Any<CacheItemOptions>());
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsTheSameDateSetInCache()
        {
            // Arrange
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
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInCache.BalanceUpDay, actual);
        }
    }
}
