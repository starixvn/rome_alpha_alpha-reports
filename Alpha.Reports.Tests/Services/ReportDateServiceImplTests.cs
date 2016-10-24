namespace Alpha.Reports.Tests.Services
{
    using System;
    using Alpha.Reports.Models;
    using Criteria;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using NSubstitute;
    using NUnit.Framework;
    using Reports.Services;
    using Reports.Services.Impl;

    [TestFixture]
    public class ReportDateServiceImplTests
    {
        private const string BalanceUpDayAndMoveBetsDayCacheKey = "Alpha.Reports.BalanceUpDayAndMoveBetsDay";
        private IDynamicRepository repository;
        private ReportDateService service;
        private DateTime balanceUpDay;
        private DateTime moveBetsDay;

        [SetUp]
        public void Setup()
        {
            CacheService.Remove(BalanceUpDayAndMoveBetsDayCacheKey);
            repository = Substitute.For<IDynamicRepository>();
            service = new ReportDateServiceImpl(repository);
            balanceUpDay = new DateTime(2000, 1, 1);
            moveBetsDay = new DateTime(2010, 1, 1);
        }

        [Test]
        public void GetBalanceUpDayAndMoveBetsDay_DataInCache_ReturnDataGetFromCache()
        {
            // Arrange            
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);            

            // Act
            var actual = service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            Assert.AreEqual(balanceUpDay, actual.BalanceUpDay);
            Assert.AreEqual(moveBetsDay, actual.MoveBetsDay);
        }

        [Test]
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

        [Test]
        public void GetBalanceUpDayAndMoveBetsDay_DataReturnedFromRepository_WasAddedToCache()
        {
            // Arrange
            var reportDates = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);            
            repository
                .Get<BalanceUpDayAndMoveBetsDay>(Arg.Any<GetBalanceUpDayAndMoveBetsDayCriteria>())
                .Returns(reportDates);            

            // Act
            service.GetBalanceUpDayAndMoveBetsDay();

            // Assert
            var dataInCache = CacheService.Get<BalanceUpDayAndMoveBetsDay>(BalanceUpDayAndMoveBetsDayCacheKey);
            Assert.AreEqual(dataInCache.BalanceUpDay, balanceUpDay);
            Assert.AreEqual(dataInCache.MoveBetsDay, moveBetsDay);
        }

        [Test]
        public void GetBalanceUpDay_Always_ReturnTheSameDateSetToCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetBalanceUpDay();

            // Assert
            Assert.AreEqual(dataInCache.BalanceUpDay, actual);            
        }

        [Test]
        public void GetMoveBetsDay_Always_ReturnTheSameDateSetToCache()
        {
            // Arrange
            var dataInCache = new BalanceUpDayAndMoveBetsDay(balanceUpDay, moveBetsDay);
            CacheService.Set(BalanceUpDayAndMoveBetsDayCacheKey, dataInCache);

            // Act
            var actual = service.GetMoveBetsDay();

            // Assert
            Assert.AreEqual(dataInCache.MoveBetsDay, actual);
        }
    }
}
