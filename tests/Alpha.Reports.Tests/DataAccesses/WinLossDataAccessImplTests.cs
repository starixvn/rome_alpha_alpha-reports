namespace Alpha.Reports.Tests.DataAccesses.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLoss;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Models.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("ObjectDbCollection")]
    public class WinLossDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly WinLossDataAccessImpl winLossDataAccessImpl;

        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;

        public WinLossDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            winLossDataAccessImpl = new WinLossDataAccessImpl();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDataAccessImpl.GetWinLoss(nullCriteria));
        }

        [Fact]
        public void GetWinLoss_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(0, currentDate, currentDate, currentDate, currentDate, null, UserLevel.Super);

            var dataSet = WinLossDatabase.MasterWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Master01", 0, 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4)),
                new WinLossModel(2, "Master02", 5, 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9))
            };

            // Act
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(0, currentDate, currentDate, currentDate, currentDate, null, UserLevel.Master);

            var dataSet = WinLossDatabase.AgentWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Agent01", 0, 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null),
                new WinLossModel(2, "Agent02", 5, 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null)
            };

            // Act
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(0, currentDate, currentDate, currentDate, currentDate, null, UserLevel.Agent);

            var dataSet = WinLossDatabase.MemberWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Member01", 0, 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null),
                new WinLossModel(2, "Member02", 6, 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null)
            };

            // Act
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(0, currentDate, currentDate, currentDate, currentDate, null, UserLevel.Agent);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossModel>();

            // Act
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}