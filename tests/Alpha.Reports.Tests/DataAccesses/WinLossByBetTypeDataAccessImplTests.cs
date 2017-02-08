namespace Alpha.Reports.Tests.DataAccesses.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLossByBetType;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.WinLossByBetTypeCriteria;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("ObjectDbCollection")]
    public class WinLossByBetTypeDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly WinLossByBettypeDataAccessImpl winLossByBettypeDataAccessImpl;

        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;

        public WinLossByBetTypeDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetBetTypeName(71).Returns("Casino Games");
            cacheRefService.GetBetTypeName(1101).Returns("Premium Baccarat");
            winLossByBettypeDataAccessImpl = new WinLossByBettypeDataAccessImpl(cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByBettype_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByBetTypeCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByBettypeDataAccessImpl.GetWinLossByBetType(nullCriteria));
        }

        [Fact]
        public void GetWinLossByBettype_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByBetTypeCriteria = new WinLossByBetTypeCriteria(0, currentDate, currentDate, currentDate, UserLevel.Super);

            var dataSet = WinLossByBetTypeDatabase.MasterWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByBetTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, false, 4, 0, 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4)),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, false, 4, 5, 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9))
            };

            // Act
            var actualData = winLossByBettypeDataAccessImpl.GetWinLossByBetType(winLossByBetTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByBetTypeCriteria = new WinLossByBetTypeCriteria(0, currentDate, currentDate, currentDate, UserLevel.Master);

            var dataSet = WinLossByBetTypeDatabase.AgentWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByBetTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, false, 4, 0, 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, false, 4, 5, 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null)
            };

            // Act
            var actualData = winLossByBettypeDataAccessImpl.GetWinLossByBetType(winLossByBetTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByBetTypeCriteria = new WinLossByBetTypeCriteria(0, currentDate, currentDate, currentDate, UserLevel.Agent);

            var dataSet = WinLossByBetTypeDatabase.MemberWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByBetTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByBetTypeModel>()
            {
                new WinLossByBetTypeModel(71, "Casino Games", 22, false, 4, 0, 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null),
                new WinLossByBetTypeModel(1101, "Premium Baccarat", 22, false, 4, 6, 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null)
            };

            // Act
            var actualData = winLossByBettypeDataAccessImpl.GetWinLossByBetType(winLossByBetTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByBettype_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var winLossByBetTypeCriteria = new WinLossByBetTypeCriteria(0, currentDate, currentDate, currentDate, UserLevel.Agent);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByBetTypeCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossByBetTypeModel>();

            // Act
            var actualData = winLossByBettypeDataAccessImpl.GetWinLossByBetType(winLossByBetTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}