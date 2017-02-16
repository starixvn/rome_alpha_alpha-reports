namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Fanex.Common;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossParamTests
    {
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossParamTests()
        {
            SystemTime.Today = currentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var winLossParam = new WinLossParam(custId, currentDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super, false);

            // Act
            var actual = winLossParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossParam = new WinLossParam(0, DateTime.MinValue, currentDate, Enumerable.Empty<int>(), UserLevel.Super, false);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_HistoricReportAndNotSupportFromDate_ReturnsFirstDateOfThisMonth()
        {
            // Arrange
            var winLossParam = new WinLossParam(0, DateTime.MinValue, currentDate, Enumerable.Empty<int>(), UserLevel.Super, true);
            var expectedDate = currentDate.FirstDayOfMonth();

            // Act
            var actualDate = winLossParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate(bool isHistoricReport)
        {
            // Arrange
            DateTime fromDate = currentDate;
            var winLossParam = new WinLossParam(0, fromDate, currentDate, Enumerable.Empty<int>(), UserLevel.Super, isHistoricReport);

            var expectedDate = fromDate;

            // Act
            var actualDate = winLossParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate(bool isHistoricReport)
        {
            // Arrange
            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super, isHistoricReport);
            var expectedDate = currentDate;

            // Act
            var actualDate = winLossParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_SupportToDate_ReturnsSupportedToDate(bool isHistoricReport)
        {
            // Arrange
            DateTime toDate = currentDate;

            var winLossParam = new WinLossParam(0, DateTime.MinValue, toDate, Enumerable.Empty<int>(), UserLevel.Super, isHistoricReport);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ReportLevel_Always_ReturnsReportLevelFromConstructor()
        {
            // Arrange
            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super, false);
            var expectedReportLevel = UserLevel.Super;

            // Act
            var actualReportLevel = winLossParam.ReportLevel;

            // Assert
            Assert.Equal(expectedReportLevel, actualReportLevel);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1,2")]
        public void UserSelectedProductIds_Always_ReturnsUserSelectedProductIdsFromConstructor(string userSelectedProductIds)
        {
            // Arrange
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();

            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, userSelectedProductIdList, UserLevel.Super, false);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = winLossParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}