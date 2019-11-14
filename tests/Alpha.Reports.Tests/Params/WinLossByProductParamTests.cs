namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    public class WinLossByProductParamTests
    {
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossByProductParamTests()
        {
            SystemTime.Today = currentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var winLossByProductParam = new WinLossByProductParam(custId, currentDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super);

            // Act
            var actual = winLossByProductParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByProductParam = new WinLossByProductParam(0, DateTime.MinValue, currentDate, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByProductParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate()
        {
            // Arrange
            DateTime fromDate = currentDate;
            var winLossByProductParam = new WinLossByProductParam(0, fromDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super);

            var expectedDate = fromDate;

            // Act
            var actualDate = winLossByProductParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossByProductParam = new WinLossByProductParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossByProductParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportToDate_ReturnsSupportedToDate()
        {
            // Arrange
            DateTime toDate = currentDate;

            var winLossByProductParam = new WinLossByProductParam(
                0, 
                DateTime.MinValue, 
                toDate,
                Enumerable.Empty<int>(),
                UserLevel.Super);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossByProductParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }        
    }
}