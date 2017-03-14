namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeParamTests
    {
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossByBetTypeParamTests()
        {
            SystemTime.Today = currentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var winLossByBetTypeParam = new WinLossByProductParam(custId, currentDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super);

            // Act
            var actual = winLossByBetTypeParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByBetTypeParam = new WinLossByProductParam(0, DateTime.MinValue, currentDate, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByBetTypeParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate()
        {
            // Arrange
            DateTime fromDate = currentDate;
            var winLossByBetTypeParam = new WinLossByProductParam(0, fromDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super);

            var expectedDate = fromDate;

            // Act
            var actualDate = winLossByBetTypeParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByBetTypeParam = new WinLossByProductParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByBetTypeParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportToDate_ReturnsSupportedToDate()
        {
            // Arrange
            DateTime toDate = currentDate;

            var winLossByBetTypeParam = new WinLossByProductParam(
                0, 
                DateTime.MinValue, 
                toDate,
                Enumerable.Empty<int>(),
                UserLevel.Super);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossByBetTypeParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }        
    }
}