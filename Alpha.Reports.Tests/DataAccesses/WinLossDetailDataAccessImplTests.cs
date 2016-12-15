namespace Alpha.Reports.Tests.DataAccesses
{
    using System;
    using System.Data;
    using Database.WinLossDetail;
    using Fanex.Data;
    using NSubstitute;
    using Reports.Criteria.WinLossDetail;
    using Reports.DataAccesses.Impl;
    using Reports.Enumerations;
    using Reports.Models;
    using Reports.Services;
    using Xunit;

    public class WinLossDetailDataAccessImplTests
    {
        private readonly IObjectDb objectDb;
        private readonly WinLossDetailDataAccessImpl winLossDetailDataAccessImpl;

        public WinLossDetailDataAccessImplTests()
        {
            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.CreateObjectDbFunc = (settingKey) => objectDb;
            winLossDetailDataAccessImpl = new WinLossDetailDataAccessImpl();
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
        public void GetData_NotNullCriteria_ReturnsDataFromObjectDb()
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

            // Act 
            var actualData = winLossDetailDataAccessImpl.GetData(winLossDetailCriteria);

            // Assert
            Assert.Equal(actualData, actualData);
        }
    }
}
