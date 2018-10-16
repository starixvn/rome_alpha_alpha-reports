namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Fanex.Common;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public class WinLossAnalysisParamTests
    {
        private readonly DateTime currentDate = new DateTime(2016, 12, 28);

        public WinLossAnalysisParamTests()
        {
            SystemTime.Today = currentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            int custId = 1;
            var param = new WinLossAnalysisParam(
                custId: custId, 
                reportType: 2, 
                fromDate: currentDate, 
                toDate: currentDate, 
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(), 
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

            // Act
            var actual = param.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.MinValue,
                toDate: currentDate,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(),
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

            var expectedDate = currentDate;

            // Act
            var actualDate = param.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate()
        {
            // Arrange
            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.MinValue,
                toDate: DateTime.MinValue,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(),
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

            var expectedDate = currentDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ToDate_SupportToDate_ReturnsSupportedToDate()
        {
            // Arrange
            DateTime toDate = currentDate;

            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.MinValue,
                toDate: toDate,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(),
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

            var expectedDate = toDate;

            // Act
            var actualDate = param.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ReportLevel_Always_ReturnsReportLevelFromConstructor()
        {
            // Arrange
            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.MinValue,
                toDate: DateTime.MinValue,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: Enumerable.Empty<int>(),
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

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

            var param = new WinLossAnalysisParam(
                custId: 0,
                reportType: 2,
                fromDate: DateTime.MinValue,
                toDate: DateTime.MinValue,
                limitedDate: DateTime.MinValue,
                userSelectedProductIds: userSelectedProductIdList,
                custLevel: UserLevel.Super,
                userSelectedExcludeProductIds: Enumerable.Empty<int>());

            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = param.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }
    }
}