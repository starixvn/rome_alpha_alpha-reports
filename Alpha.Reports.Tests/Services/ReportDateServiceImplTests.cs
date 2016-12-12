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
        private IDynamicRepository repository;
        private ReportDateService service;
        private DateTime balanceUpDay;
        private DateTime moveBetsDay;

        public ReportDateServiceImplTests()
        {
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
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.Equal(balanceUpDay, actual.BalanceUpDay);
            Assert.Equal(moveBetsDay, actual.MoveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDayAndMoveBetsDay_NoDataInCache_DynamicRepositoryGetWasCalled()
        {
            // Arrange            
            CacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);
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
            var dataInCache = CacheService.Get<BalanceUpDayAndMoveBetsDay>(BalanceUpDayAndMoveBetsDayCacheKey);
            Assert.Equal(dataInCache.BalanceUpDay, balanceUpDay);
            Assert.Equal(dataInCache.MoveBetsDay, moveBetsDay);
        }

        [Fact]
        public void GetBalanceUpDay_Always_ReturnsTheSameDateSetToCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.Equal(dataInCache.BalanceUpDay, actual);
        }

        [Fact]
        public void GetMoveBetsDay_Always_ReturnsTheSameDateSetToCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetMoveBetsDay();

            // Assert
            Assert.Equal(dataInCache.MoveBetsDay, actual);
        }
    }
}
