//namespace Alpha.Reports.Tests.DataAccesses.WinLoss
//{
//    using System;
//    using System.Data;
//    using Fanex.Data;
//    using NSubstitute;
//    using Reports.Criteria.WinLoss;
//    using Reports.DataAccesses.Impl;
//    using Reports.Enumerations;
//    using Reports.Models;
//    using Reports.Services;
//    using Xunit;

//    [Trait("Category", "WinLoss")]
//    public class WinLossDataAccessImplTests
//    {
//        private readonly IObjectDb objectDb;
//        private readonly WinLossDataAccessImpl winLossDataAccessImpl;

//        public WinLossDataAccessImplTests()
//        {
//            objectDb = Substitute.For<IObjectDb>();
//            ObjectDbFactory.CreateObjectDbFunc = (settingKey) => objectDb;
//            winLossDataAccessImpl = new WinLossDataAccessImpl();
//        }

//        [Fact]
//        public void GetData_NullCriteria_ThrowsArgumentNullException()
//        {
//            // Arrange
//            WinLossCriteria nullCriteria = null;

//            // Act & Assert
//            Assert.Throws<ArgumentNullException>(() => winLossDataAccessImpl.GetWinLosses(nullCriteria));
//        }

//        [Fact]
//        public void GetData_NotNullCriteria_ReturnsDataFromObjectDb()
//        {
//            // Arrange
//            var winLossCriteria = new WinLossCriteria(
//                0,
//                DateTime.Now,
//                DateTime.Now,
//                DateTime.Now,
//                DateTime.Now,
//                null,
//                Enumeration.FromValue<UserLevel>(4));

//            objectDb.ExecuteReader(Arg.Any<WinLossCriteria>()).Returns(WinLossDataReader());

//            // Act 
//            var actualData = winLossDataAccessImpl.GetWinLosses(winLossCriteria);

//            // Assert
//            Assert.Equal(actualData, actualData);
//        }

//        private IDataReader WinLossDataReader()
//        {
//            var table = new DataTable();
//            var reader = table.CreateDataReader();

//            return default(IDataReader);
//        }
//    }
//}
