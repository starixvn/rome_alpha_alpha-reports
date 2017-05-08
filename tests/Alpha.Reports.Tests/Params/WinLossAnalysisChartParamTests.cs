namespace Alpha.Reports.Tests.Params
{
    using Constants;
    using Fanex.Common;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using System;
    using System.Linq;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    public class WinLossAnalysisChartParamTests
    {
        public WinLossAnalysisChartParamTests()
        {
            SystemTime.Today = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromConstructor()
        {
            // Arrange
            var custId = 1;
            var winLossAnalysisChartParam = new WinLossAnalysisChartParam(custId, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), UserLevel.Super);

            // Act
            var actual = winLossAnalysisChartParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_NotSupportFromDate_ReturnsFirstDateOfLastTwoMonths()
        {
            // Arrange
            var param = new WinLossAnalysisChartParam(0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = SystemTime.Today.AddMonths(-2).FirstDayOfMonth();

            // Act
            var actualDate = param.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_SupportFromDate_ReturnsFirstDayOfFromDateMonth()
        {
            // Arrange
            var fromDate = DefaultVariables.CurrentDate;
            var param = new WinLossAnalysisChartParam(0, fromDate, DateTime.Now, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = fromDate.FirstDayOfMonth();

            // Act
            var actualDate = param.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new WinLossAnalysisChartParam(0, DateTime.Now, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = SystemTime.Today;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportToDate_ReturnsSuportedDate()
        {
            // Arrange
            var toDate = DefaultVariables.CurrentDate;
            var param = new WinLossAnalysisChartParam(0, DateTime.Now, toDate, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = toDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void Todate_SupportToDate_ReturnsLastDayOfToDateMonth()
        {
            // Arrange
            var toDate = DefaultVariables.CurrentDate.AddMonths(-1);
            var param = new WinLossAnalysisChartParam(0, DateTime.Now, toDate, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = toDate.LastDayOfMonth();

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ReportLevel_Always_ReturnsReportLevelFromConstructor()
        {
            // Arrange
            var param = new WinLossAnalysisChartParam(0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedCustLevel = UserLevel.Super;

            // Act
            var actualCustLevel = param.CustLevel;

            // Assert
            Assert.Equal(expectedCustLevel, actualCustLevel);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1,2")]
        public void UserSelectedProductIds_Always_ReturnsUserSelectedProductIdsFromConstructor(string userSelectedProductIds)
        {
            // Arrange
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();
            var param = new WinLossAnalysisChartParam(0, DateTime.Now, DateTime.Now, userSelectedProductIdList, UserLevel.Super);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = param.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}