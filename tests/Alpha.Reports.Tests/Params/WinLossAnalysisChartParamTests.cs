namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Constants;
    using Fanex.Common;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    public class WinLossAnalysisChartParamTests
    {
        public WinLossAnalysisChartParamTests()
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromConstructor()
        {
            // Arrange
            var custId = 1;
            var winLossAnalysisChartParam = new WinLossAnalysisChartParam(custId, "", 1, DateTime.Now, DateTime.Now, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);

            // Act
            var actual = winLossAnalysisChartParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_NotSupportFromDate_ReturnsFirstDateOfLastTwoMonths()
        {
            // Arrange
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.MinValue, DateTime.Now, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
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
            var fromDate = DefaultVariablesTest.CurrentDate;
            var param = new WinLossAnalysisChartParam(0, "", 1, fromDate, DateTime.Now, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
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
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.Now, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = SystemTime.Today;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportCurrentDate_ReturnsSuportedDate()
        {
            // Arrange
            var toDate = DefaultVariablesTest.CurrentDate;
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.Now, toDate, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
            var expectedDate = toDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void Todate_SupportNotCurrentDate_ReturnsLastDayOfToDateMonth()
        {
            // Arrange
            var toDate = DefaultVariablesTest.CurrentDate.AddMonths(-1);
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.Now, toDate, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
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
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.Now, DateTime.Now, DateTime.MinValue, Enumerable.Empty<int>(), UserLevel.Super);
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
            var param = new WinLossAnalysisChartParam(0, "", 1, DateTime.Now, DateTime.Now, DateTime.MinValue, userSelectedProductIdList, UserLevel.Super);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = param.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}