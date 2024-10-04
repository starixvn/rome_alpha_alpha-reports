namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLossDetail;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("ObjectDbCollection")]
    public class WinLossDetailDataAccessImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly WinLossDetailDataAccessImpl winLossDetailDataAccessImpl;
        private readonly CompareLogic compareLogic;

        public WinLossDetailDataAccessImplTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            winLossDetailDataAccessImpl = new WinLossDetailDataAccessImpl(repository);
            compareLogic = new CompareLogic();
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
                1,
                1,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1,2",
                UserLevel.Super,
                string.Empty,
                string.Empty,
                false,
                string.Empty);

            var dataSet = (
                new List<WinLossDetailEntity> { },
                new List<WinLossDetailEntity> { }
            );
            repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>
            {
                new WinLossDetailModel(1, "Master1", string.Empty, string.Empty, false, 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), new NetWinLoss(9, 10), 0),
                new WinLossDetailModel(11, "Master2", string.Empty, string.Empty, false, 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), new NetWinLoss(19, 110), 0)
            };

            var expectedData = new PagedReportData<WinLossDetailModel>(
                winLossDetails,
                1,
                0,
                0,
                new WinLossDetailModel(0, "Total", null, null, false, 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), new NetWinLoss(27, 28), new NetWinLoss(29, 210), 0));

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
                1,
                1,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1,2",
                UserLevel.Master,
                string.Empty,
                string.Empty,
                false,
                string.Empty);

            var dataSet = (
                new List<WinLossDetailEntity> { },
                new List<WinLossDetailEntity> { }
            );
            repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Agent1", string.Empty, string.Empty, false, 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), null, 0),
                new WinLossDetailModel(11, "Agent2", string.Empty, string.Empty, false, 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), null, 0)
            };

            var expectedData = new PagedReportData<WinLossDetailModel>(
                winLossDetails,
                1,
                0,
                0,
                new WinLossDetailModel(0, "Total", null, null, false, 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), new NetWinLoss(27, 28), null, 0));

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
                1,
                1,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1,2",
                UserLevel.Agent,
                string.Empty,
                string.Empty,
                false,
                string.Empty);

            var dataSet = (
                new List<WinLossDetailEntity> { },
                new List<WinLossDetailEntity> { }
            );
            repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Member1", string.Empty, string.Empty, false, 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), null, null, 0),
                new WinLossDetailModel(11, "Member2", string.Empty, string.Empty, false, 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), null, null, 0)
            };

            var expectedData = new PagedReportData<WinLossDetailModel>(
                winLossDetails,
                1,
                0,
                0,
                new WinLossDetailModel(0, "Total", null, null, false, 22, 21, 20, 211, new NetWinLoss(23, 24), new NetWinLoss(25, 26), null, null, 0));

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
                UserLevel.Super,
                string.Empty,
                string.Empty,
                false,
                string.Empty);

            var dataSet = (
                new List<WinLossDetailEntity> { },
                new List<WinLossDetailEntity> { }
            );
            repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var expectedData = new PagedReportData<WinLossDetailModel>(
                new List<WinLossDetailModel>(),
                0,
                0,
                0,
                new WinLossDetailModel());

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }

        [Fact]
        public void GetData_MasterWinLossReportHasDataWinNoTotalTable_ReturnsDataFromObjectDbAndDefaultTotal()
        {
            // Arrange
            var winLossDetailCriteria = new WinLossDetailCriteria(
                1,
                1,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                "1,2",
                UserLevel.Super,
                string.Empty,
                string.Empty,
                false,
                string.Empty);

            var dataSet = (
                new List<WinLossDetailEntity> { },
                new List<WinLossDetailEntity> { }
            );
            repository.FetchMultiple<WinLossDetailEntity, WinLossDetailEntity>(Arg.Any<WinLossDetailCriteria>()).Returns(dataSet);

            var winLossDetails = new List<WinLossDetailModel>()
            {
                new WinLossDetailModel(1, "Master1", string.Empty, string.Empty, false, 2, 1, 0, 11, new NetWinLoss(3, 4), new NetWinLoss(5, 6), new NetWinLoss(7, 8), new NetWinLoss(9, 10), 0),
                new WinLossDetailModel(11, "Master2", string.Empty, string.Empty, false, 12, 11, 10, 111, new NetWinLoss(13, 14), new NetWinLoss(15, 16), new NetWinLoss(17, 18), new NetWinLoss(19, 110), 0)
            };

            var expectedData = new PagedReportData<WinLossDetailModel>(
                winLossDetails,
                1,
                0,
                0,
                new WinLossDetailModel());

            // Act
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            Assert.True(compareLogic.Compare(expectedData, actualData).AreEqual);
        }
    }
}