namespace Alpha.Reports.Tests.DataAccesses.WinLossByProduct
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLossByProduct;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossByProductCriteria;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("ObjectDbCollection")]
    public class WinLossByProductDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly WinLossByProductDataAccessImpl winLossByProductDataAccessImpl;

        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;

        public WinLossByProductDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;

            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetBetTypeName(71).Returns("Casino Games");
            cacheRefService.GetBetTypeName(1101).Returns("Premium Baccarat");

            winLossByProductDataAccessImpl = new WinLossByProductDataAccessImpl(cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByBettype_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByProductCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByProductDataAccessImpl.GetWinLossByBetType(nullCriteria));
        }

        [Fact]
        public void GetWinLossByBettype_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByProductCriteria = new WinLossByProductCriteria(1, currentDate, currentDate, currentDate, currentDate, new List<int> { 1, 2 }, UserLevel.Super, string.Empty);

            var dataSet = WinLossByProductDatabase.MasterWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByProductCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, 1, string.Empty, false, 4, 0, 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4)),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, 0, string.Empty, false, 4, 5, 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9))
            };

            // Act
            var actualData = winLossByProductDataAccessImpl.GetWinLossByBetType(winLossByProductCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByProductCriteria = new WinLossByProductCriteria(1, currentDate, currentDate, currentDate, currentDate, new List<int> { 1, 2 }, UserLevel.Master, string.Empty);

            var dataSet = WinLossByProductDatabase.AgentWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByProductCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, 0, string.Empty, false, 4, 0, 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, 0, string.Empty, false, 4, 5, 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null)
            };

            // Act
            var actualData = winLossByProductDataAccessImpl.GetWinLossByBetType(winLossByProductCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByProductCriteria = new WinLossByProductCriteria(1, currentDate, currentDate, currentDate, currentDate, new List<int> { 1, 2 }, UserLevel.Agent, string.Empty);

            var dataSet = WinLossByProductDatabase.MemberWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByProductCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, 0, string.Empty, false, 4, 0, 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, 0, string.Empty, false, 4, 6, 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null)
            };

            // Act
            var actualData = winLossByProductDataAccessImpl.GetWinLossByBetType(winLossByProductCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var winLossByProductCriteria = new WinLossByProductCriteria(0, currentDate, currentDate, currentDate, currentDate, null, UserLevel.Agent, string.Empty);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByProductCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossByBetTypeModel>();

            // Act
            var actualData = winLossByProductDataAccessImpl.GetWinLossByBetType(winLossByProductCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}