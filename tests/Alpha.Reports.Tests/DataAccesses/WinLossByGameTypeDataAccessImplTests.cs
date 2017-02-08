namespace Alpha.Reports.Tests.DataAccesses.WinLossByGameType
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLossByGameType;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using RemotingRef;
    using Reports.Criteria.WinLossByGameType;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinlossByGameType;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    [Collection("ObjectDbCollection")]
    public class WinLossByGameTypeDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly WinLossByGameTypeDataAccessImpl winLossByGameTypeDataAccessImpl;

        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;
        private readonly CacheRefService cacheRefService;

        public WinLossByGameTypeDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetBetTypeName(2176).Returns("Sea Orchestra");
            cacheRefService.GetBetTypeName(2301).Returns("(Mobile)Baccarat");
            winLossByGameTypeDataAccessImpl = new WinLossByGameTypeDataAccessImpl(cacheRefService);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetWinLossByGameType_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossByGameTypeCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossByGameTypeDataAccessImpl.GetWinLossByGameType(nullCriteria));
        }

        [Fact]
        public void GetWinLossByGameType_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByGameTypeCriteria = new WinLossByGameTypeCriteria(0, 0, currentDate, currentDate, currentDate, UserLevel.Super);

            var dataSet = WinLossByGameTypeDatabase.MasterWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByGameTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByGameTypeModel>()
            {
                new WinLossByGameTypeModel(2176, "Sea Orchestra", 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4)),
                new WinLossByGameTypeModel(2301, "(Mobile)Baccarat", 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9))
            };

            // Act
            var actualData = winLossByGameTypeDataAccessImpl.GetWinLossByGameType(winLossByGameTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByGameType_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByGameTypeCriteria = new WinLossByGameTypeCriteria(0, 0, currentDate, currentDate, currentDate, UserLevel.Master);

            var dataSet = WinLossByGameTypeDatabase.AgentWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByGameTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByGameTypeModel>()
            {
                new WinLossByGameTypeModel(2176, "Sea Orchestra", 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null),
                new WinLossByGameTypeModel(2301, "(Mobile)Baccarat", 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null)
            };

            // Act
            var actualData = winLossByGameTypeDataAccessImpl.GetWinLossByGameType(winLossByGameTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByGameType_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossByGameTypeCriteria = new WinLossByGameTypeCriteria(0, 0, currentDate, currentDate, currentDate, UserLevel.Agent);

            var dataSet = WinLossByGameTypeDatabase.MemberWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByGameTypeCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossByGameTypeModel>()
            {
                new WinLossByGameTypeModel(2176, "Sea Orchestra", 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null),
                new WinLossByGameTypeModel(2301, "(Mobile)Baccarat", 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null)
            };

            // Act
            var actualData = winLossByGameTypeDataAccessImpl.GetWinLossByGameType(winLossByGameTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLossByGameType_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var winLossByGameTypeCriteria = new WinLossByGameTypeCriteria(0, 0, currentDate, currentDate, currentDate, UserLevel.Agent);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossByGameTypeCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossByGameTypeModel>();

            // Act
            var actualData = winLossByGameTypeDataAccessImpl.GetWinLossByGameType(winLossByGameTypeCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}