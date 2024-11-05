namespace Alpha.Reports.Tests.DataAccesses.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("ObjectDbCollection")]
    public class WinLossDataAccessImplTests
    {
        private readonly IDynamicRepository repository;
        private readonly WinLossDataAccessImpl winLossDataAccessImpl;

        private readonly DateTime currentDate = new DateTime(2016, 12, 28);
        private readonly CompareLogic compareLogic;

        public WinLossDataAccessImplTests()
        {
            repository = Substitute.For<IDynamicRepository>();
            winLossDataAccessImpl = new WinLossDataAccessImpl(repository);
            compareLogic = new CompareLogic();
        }

        [Fact]
        public void GetData_NullCriteria_ThrowsArgumentNullException()
        {
            // Arrange
            WinLossMemberCriteria nullCriteria = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => winLossDataAccessImpl.GetWinLoss(nullCriteria));
        }

        [Fact]
        public void GetWinLoss_MasterWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossMemberCriteria(1, currentDate, currentDate, currentDate, currentDate, "1,2", string.Empty);

            var dataSet = new List<WinLossEntity>
            {
                new WinLossEntity
                {
                    CustId = 1,
                    CustName = "Master01",
                    Turnover = 0,
                    GrossComm = 1,
                    DownlineTotal = 2,
                    WinLoss = 3,
                    Comm = 4
                },
                new WinLossEntity
                {
                    CustId = 2,
                    CustName = "Master02",
                    Turnover = 5,
                    GrossComm = 6,
                    DownlineTotal = 7,
                    WinLoss = 8,
                    Comm = 9
                }
            };
            repository.Fetch<WinLossEntity>(Arg.Any<WinLossMemberCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Master01", 0, 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4), false),
                new WinLossModel(2, "Master02", 5, 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9), false)
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
            var winLossCriteria = new WinLossMemberCriteria(1, currentDate, currentDate, currentDate, currentDate, "1,2", string.Empty);

            var dataSet = new List<WinLossEntity>
            {
                new WinLossEntity
                {
                    CustId = 1,
                    CustName = "Agent01",
                    Turnover = 0,
                    GrossComm = 1,
                    DownlineTotal = 2,
                    WinLoss = 3,
                    Comm = 4
                },
                new WinLossEntity
                {
                    CustId = 2,
                    CustName = "Agent02",
                    Turnover = 5,
                    GrossComm = 6,
                    DownlineTotal = 7,
                    WinLoss = 8,
                    Comm = 9
                }
            };
            repository.Fetch<WinLossEntity>(Arg.Any<WinLossMemberCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Agent01", 0, 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null, false),
                new WinLossModel(2, "Agent02", 5, 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null, false)
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
            var winLossCriteria = new WinLossMemberCriteria(1, currentDate, currentDate, currentDate, currentDate, "1,2", string.Empty);

            var dataSet = new List<WinLossEntity>
            {
                new WinLossEntity
                {
                    CustId = 1,
                    CustName = "Member01",
                    Turnover = 0,
                    GrossComm = 1,
                    MemberWinLoss = 2,
                    MemberComm = 3,
                    WinLoss = 4,
                    Comm = 5
                },
                new WinLossEntity
                {
                    CustId = 2,
                    CustName = "Member02",
                    Turnover = 6,
                    GrossComm = 7,
                    MemberWinLoss = 8,
                    MemberComm = 9,
                    WinLoss = 10,
                    Comm = 11
                }
            };
            repository.Fetch<WinLossEntity>(Arg.Any<WinLossMemberCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLossModel>()
            {
                new WinLossModel(1, "Member01", 0, 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null, false),
                new WinLossModel(2, "Member02", 6, 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null, false)
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
            var winLossCriteria = new WinLossMemberCriteria(0, currentDate, currentDate, currentDate, currentDate, null, string.Empty);

            var dataReader = Enumerable.Empty<WinLossEntity>();
            repository.Fetch<WinLossEntity>(Arg.Any<WinLossMemberCriteria>()).Returns(dataReader);

            var expectedData = new List<WinLossModel>();

            // Act
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);

            // Assert
            var result = compareLogic.Compare(expectedData, actualData);
            Assert.True(result.AreEqual);
        }
    }
}