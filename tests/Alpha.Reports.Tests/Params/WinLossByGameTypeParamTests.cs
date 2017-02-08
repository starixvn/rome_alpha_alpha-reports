namespace Alpha.Reports.Tests.Params
{
    using System;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    public class WinLossByGameTypeParamTests
    {
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossByGameTypeParamTests()
        {
            SystemTime.Today = currentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var winLossByGameTypeParam = new WinLossByGameTypeParam(custId, 71, currentDate, currentDate, UserLevel.Super);

            // Act
            var actual = winLossByGameTypeParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByGameTypeParam = new WinLossByGameTypeParam(0, 71, DateTime.MinValue, currentDate, UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByGameTypeParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate()
        {
            // Arrange
            DateTime fromDate = currentDate;
            var winLossByGameTypeParam = new WinLossByGameTypeParam(0, 71, fromDate, currentDate, UserLevel.Super);

            var expectedDate = fromDate;

            // Act
            var actualDate = winLossByGameTypeParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByGameTypeParam = new WinLossByGameTypeParam(0, 71, DateTime.MinValue, DateTime.MinValue, UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByGameTypeParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportToDate_ReturnsSupportedToDate()
        {
            // Arrange
            DateTime toDate = currentDate;

            var winLossByGameTypeParam = new WinLossByGameTypeParam(0, 71, DateTime.MinValue, toDate,  UserLevel.Super);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossByGameTypeParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }        
    }
}