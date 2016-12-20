namespace Alpha.Reports.Tests.DataAccesses.WinLoss
{
    using System;
    using System.Collections.Generic;
    using Database.WinLoss;
    using Fanex.Data;
    using NSubstitute;
    using Reports.Criteria.WinLoss;
    using Reports.DataAccesses.Impl;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;
    using Reports.Models.Winloss;
    using Reports.Models.WinlossDetails;
    using Comparers.WinLoss;

    [Trait("Category", "WinLoss")]
    public class WinLossDataAccessImplTests
    {
        private readonly IObjectDb objectDb;
        private readonly WinLossDataAccessImpl winLossDataAccessImpl;

        public WinLossDataAccessImplTests()
        {
            objectDb = Substitute.For<IObjectDb>();
            winLossDataAccessImpl = new WinLossDataAccessImpl();
            winLossDataAccessImpl.SetCreateObjectDbFunc((settingKey) => objectDb);
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

            // Assert
            Assert.True(WinLossReportDataComparer.Equals(expectedData, actualData));
        }

    }
}
