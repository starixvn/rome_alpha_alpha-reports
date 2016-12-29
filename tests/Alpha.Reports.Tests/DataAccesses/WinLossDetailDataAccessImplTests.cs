namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Database.WinLossDetail;
    using Extensions;
    using Fanex.Data;
    using NSubstitute;
    using Reports.Criteria.WinLossDetail;
    using Reports.DataAccesses;
    using Reports.DataAccesses.Impl;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.WinlossDetails;
    using Xunit;

    public class WinLossDetailDataAccessImplTests
    {
        private readonly IObjectDb objectDb;
        private readonly WinLossDetailDataAccessImpl winLossDetailDataAccessImpl;

        public WinLossDetailDataAccessImplTests()
        {
            objectDb = Substitute.For<IObjectDb>();
            winLossDetailDataAccessImpl = new WinLossDetailDataAccessImpl();
            ObjectDbFactory.CreateInstance = (settingKey) => objectDb;
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossDetailCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDetailDataAccessImpl.GetData(nullCriteria));
        }

        [Fact]
        public void GetData_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(4));

            var dataSet = WinLossDetailDatabase.MasterWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Master1", 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), new NetWinLoss(9, 10)),
                new WinLossDetailModel(11, "Master2", 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), new NetWinLoss(19, 110))
            };

            var expectedData = new ReportData<WinLossDetailModel>(
                winLossDetails,
                0,
                0,
                0,
                new WinLossDetailModel(0, "Total", 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), new NetWinLoss(27, 28), new NetWinLoss(29, 210)));

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(3));

            var dataSet = WinLossDetailDatabase.AgentWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Agent1", 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), null),
                new WinLossDetailModel(11, "Agent2", 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), null)
            };

            var expectedData = new ReportData<WinLossDetailModel>(
                winLossDetails,
                0,
                0,
                0,
                new WinLossDetailModel(0, "Total", 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), new NetWinLoss(27, 28), null));

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(2));

            var dataSet = WinLossDetailDatabase.MemberWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Member1", 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), null, null),
                new WinLossDetailModel(11, "Member2", 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), null, null)
            };

            var expectedData = new ReportData<WinLossDetailModel>(
                winLossDetails,
                0,
                0,
                0,
                new WinLossDetailModel(0, "Total", 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), null, null));

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_NullDataReturnFromDb_ReturnsDefaultData()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(4));

            IDataReader dataReader = null;
            objectDb.ExecuteReader(Arg.Any<WinLossDetailCriteria>()).Returns(dataReader);

            var expectedData = new ReportData<WinLossDetailModel>(
                new List<WinLossDetailModel>(),
                0,
                0,
                0,
                new WinLossDetailModel());

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }

        [Fact]
        public void GetData_MasterWinLossReportHasDataWinNoTotalTable_ReturnsDataFromObjectDbAndDefaultTotal()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                0,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(4));

            var dataSet = WinLossDetailDatabase.MasterWinLossWinNoTotalTable.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Master1", 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), new NetWinLoss(9, 10)),
                new WinLossDetailModel(11, "Master2", 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), new NetWinLoss(19, 110))
            };

            var expectedData = new ReportData<WinLossDetailModel>(
                winLossDetails,
                0,
                0,
                0,
                new WinLossDetailModel());

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            AssertExtension.Equal(expectedData, actualData);
        }
    }
}
