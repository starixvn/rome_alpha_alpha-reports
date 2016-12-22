namespace Alpha.Reports.Tests.DataAccesses.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Database.WinLoss;
    using Fanex.Data;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses;
    using Reports.DataAccesses.Impl;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Reports.Models.WinlossDetails;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossDataAccessImplTests
    {
        private readonly IObjectDb objectDb;
        private readonly WinLossDataAccessImpl winLossDataAccessImpl;
        private readonly CompareLogic _compare;

        public WinLossDataAccessImplTests()
        {
            objectDb = Substitute.For<IObjectDb>();
            winLossDataAccessImpl = new WinLossDataAccessImpl();
            ObjectDbFactory.CreateInstance = (settingKey) => objectDb;
            _compare = new CompareLogic();
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
            var winLossCriteria = new WinLossCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(4));

            var dataSet = WinLossDatabase.MasterWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLoss>()
            {
                new WinLoss(1, "Master01", 0, 1, null, null, new NetWinLoss(2), new NetWinLoss(3, 4)),
                new WinLoss(2, "Master02", 5, 6, null, null, new NetWinLoss(7), new NetWinLoss(8, 9))
            };

            // Act 
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);
            var result = _compare.Compare(expectedData, actualData);

            // Assert
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_AgentWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(3));

            var dataSet = WinLossDatabase.AgentWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLoss>()
            {
                new WinLoss(1, "Agent01", 0, 1, null, new NetWinLoss(2), new NetWinLoss(3, 4), null),
                new WinLoss(2, "Agent02", 5, 6, null, new NetWinLoss(7), new NetWinLoss(8, 9), null)
            };

            // Act 
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);
            var result = _compare.Compare(expectedData, actualData);

            // Assert
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void GetWinLoss_MemberWinLossReportHasData_ReturnsDataFromObjectDb()
        {
            // Arrange
            var winLossCriteria = new WinLossCriteria(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                null,
                Enumeration.FromValue<UserLevel>(2));

            var dataSet = WinLossDatabase.MemberWinLoss.CreateDataReader();
            objectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(dataSet);

            var expectedData = new List<WinLoss>()
            {
                new WinLoss(1, "Member01", 0, 1, new NetWinLoss(2, 3), new NetWinLoss(4, 5), null, null),
                new WinLoss(2, "Member02", 6, 7, new NetWinLoss(8, 9), new NetWinLoss(10, 11), null, null)
            };

            // Act 
            var actualData = winLossDataAccessImpl.GetWinLoss(winLossCriteria);
            var result = _compare.Compare(expectedData, actualData);

            // Assert
            Assert.True(result.AreEqual);
        }
    }
}
