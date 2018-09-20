namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.PokerGamesWinLossDetail;
    using Extensions;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.PokerGamesWinLossDetail;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Reports.Params;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    [Collection("ObjectDbCollection")]
    public class PokerGamesWinLossDetailDataAccessImplTests
    {
        private readonly ObjectDbFixture objectDbFixture;
        private readonly PokerGamesWinLossDetailDataAccessImpl winLossDetailDataAccessImpl;
        private readonly CompareLogic compareLogic;

        public PokerGamesWinLossDetailDataAccessImplTests(ObjectDbFixture objectDbFixture)
        {
            this.objectDbFixture = objectDbFixture;
            winLossDetailDataAccessImpl = new PokerGamesWinLossDetailDataAccessImpl();
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            PokerGamesWinLossDetailCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailDataAccessImpl.GetData(nullCriteria));
        }

        [Fact]
        public void GetData_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(1, 1, 0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            var dataSet = PokerGamesWinLossDetailDatabase.MasterPokerGamesWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<PokerGamesWinLossDetailModel>
            {
                new PokerGamesWinLossDetailModel(1, "Master1", 2, 1, 0, 11, new PokerGamesWinLoss(3, 20, 4), new PokerGamesWinLoss(0, 5, 6), new PokerGamesWinLoss(0, 7, 8), new PokerGamesWinLoss(0, 9, 10)),
                new PokerGamesWinLossDetailModel(11, "Master2", 12, 11, 10, 111, new PokerGamesWinLoss(13, 20, 14), new PokerGamesWinLoss(0, 15, 16), new PokerGamesWinLoss(0, 17, 18), new PokerGamesWinLoss(0, 19, 110))
            };

            var expectedData = new PagedReportData<PokerGamesWinLossDetailModel>(
                winLossDetails,
                1,
                1000,
                0,
                new PokerGamesWinLossDetailModel(0, "Total", 22, 21, 20, 211, new PokerGamesWinLoss(23, 20, 24), new PokerGamesWinLoss(0, 25, 26), new PokerGamesWinLoss(0, 27, 28), new PokerGamesWinLoss(0, 29, 210)));

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(1, 1, 0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Master, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            var dataSet = PokerGamesWinLossDetailDatabase.AgentPokerGamesWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<PokerGamesWinLossDetailModel>()
            {
                new PokerGamesWinLossDetailModel(1, "Agent1", 2, 1, 0, 11, new PokerGamesWinLoss(3, 20, 4), new PokerGamesWinLoss(0, 5, 6), new PokerGamesWinLoss(0, 7, 8), null),
                new PokerGamesWinLossDetailModel(11, "Agent2", 12, 11, 10, 111, new PokerGamesWinLoss(13, 20, 14), new PokerGamesWinLoss(0, 15, 16), new PokerGamesWinLoss(0, 17, 18), null)
            };

            var expectedData = new PagedReportData<PokerGamesWinLossDetailModel>(
                winLossDetails,
                1,
                1000,
                0,
                new PokerGamesWinLossDetailModel(0, "Total", 22, 21, 20, 211, new PokerGamesWinLoss(23, 20, 24), new PokerGamesWinLoss(0, 25, 26), new PokerGamesWinLoss(0, 27, 28), null));

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(1, 1, 0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Agent, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            var dataSet = PokerGamesWinLossDetailDatabase.MemberPokerGamesWinLoss.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<PokerGamesWinLossDetailModel>()
            {
                new PokerGamesWinLossDetailModel(1, "Member1", 2, 1, 0, 11, new PokerGamesWinLoss(3, 20, 4), new PokerGamesWinLoss(0, 5, 6), null, null),
                new PokerGamesWinLossDetailModel(11, "Member2", 12, 11, 10, 111, new PokerGamesWinLoss(13, 20, 14), new PokerGamesWinLoss(0, 15, 16), null, null)
            };

            var expectedData = new PagedReportData<PokerGamesWinLossDetailModel>(
                winLossDetails,
                1,
                1000,
                0,
                new PokerGamesWinLossDetailModel(0, "Total", 22, 21, 20, 211, new PokerGamesWinLoss(23, 20, 24), new PokerGamesWinLoss(0, 25, 26), null, null));

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            IDataReader dataReader = null;
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(dataReader);

            var expectedData = new PagedReportData<PokerGamesWinLossDetailModel>(
                new List<PokerGamesWinLossDetailModel>(),
                1,
                1000,
                0,
                new PokerGamesWinLossDetailModel());

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetData_MasterWinLossReportHasDataWinNoTotalTable_ReturnsDataFromObjectDbAndDefaultTotal()
        {
            // Arrange
            var param = new PokerGamesWinLossDetailParam(1, 1, 0, DateTime.Now, DateTime.Now, DateTime.Now, UserLevel.Super, string.Empty, false, false);
            var winLossDetailCriteria = new PokerGamesWinLossDetailCriteria(param, DateTime.Now, DateTime.Now);

            var dataSet = PokerGamesWinLossDetailDatabase.MasterPokerGamesWinLossWinNoTotalTable.CreateDataReader();
            objectDbFixture.ObjectDb.ExecuteReader(Arg.Any<PokerGamesWinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<PokerGamesWinLossDetailModel>()
            {
                new PokerGamesWinLossDetailModel(1, "Master1", 2, 1, 0, 11, new PokerGamesWinLoss(3, 20, 4), new PokerGamesWinLoss(0, 5, 6), new PokerGamesWinLoss(0, 7, 8), new PokerGamesWinLoss(0, 9, 10)),
                new PokerGamesWinLossDetailModel(11, "Master2", 12, 11, 10, 111, new PokerGamesWinLoss(13, 20, 14), new PokerGamesWinLoss(0, 15, 16), new PokerGamesWinLoss(0, 17, 18), new PokerGamesWinLoss(0, 19, 110))
            };

            var expectedData = new PagedReportData<PokerGamesWinLossDetailModel>(
                winLossDetails,
                1,
                1000,
                0,
                new PokerGamesWinLossDetailModel());

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}