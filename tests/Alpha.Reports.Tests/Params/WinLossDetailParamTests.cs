namespace Alpha.Reports.Tests.Params
{
    using System;
    using System.Linq;
    using Constants;
    using Fanex.Common;
    using Reports.Enumerations;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Params;
    using Reports.Utils;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailParamTests
    {
        public WinLossDetailParamTests()
        {
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            var custId = 1;
            var winLossDetailParam = new WinLossDetailParam(
                custId,
                custId,
                string.Empty,
                1,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);

            // Act
            var actual = winLossDetailParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedDate = DefaultVariablesTest.CurrentDate;

            // Act
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_HistoricReportAndNotSupportFromDate_ReturnsFirstDateOfThisMonth()
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                true,
                string.Empty);
            var expectedDate = DefaultVariablesTest.CurrentDate.FirstDayOfMonth();

            // Act
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FromDate_SupportFromDate_ReturnsSupportedFromDate(bool isHistoricReport)
        {
            // Arrange
            DateTime fromDate = DefaultVariablesTest.CurrentDate;

            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                fromDate,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                isHistoricReport,
                string.Empty);
            var expectedDate = fromDate;

            // Act
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_NotSupportToDate_ReturnsCurrentDate(bool isHistoricReport)
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                    0,
                    1,
                    "",
                    1,
                    0,
                    0,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    Enumerable.Empty<int>(),
                    Enumerable.Empty<int>(),
                    UserLevel.Super,
                    string.Empty,
                    false,
                    isHistoricReport,
                    string.Empty);

            var expectedDate = DefaultVariablesTest.CurrentDate;

            // Act
            var actualDate = winLossDetailParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ToDate_SupportToDate_ReturnsSupportedToDate(bool isHistoricReport)
        {
            // Arrange
            DateTime toDate = DefaultVariablesTest.CurrentDate;

            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                toDate,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                isHistoricReport,
                string.Empty);
            var expectedDate = toDate;

            // Act
            var actualDate = winLossDetailParam.ToDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void ReportLevel_Always_ReturnsReportLevelFromConstructor()
        {
            // Arrange
            var custLevel = UserLevel.Super;
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                custLevel,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedReportLevel = custLevel;

            // Act
            var actualReportLevel = winLossDetailParam.ReportLevel;

            // Assert
            Assert.Equal(expectedReportLevel, actualReportLevel);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsDescendingSort_Always_ReturnsISDescendingSortFromConstructor(bool isDescendingSort)
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                "",
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                isDescendingSort,
                false,
                string.Empty);
            var expectedIsDescendingSort = isDescendingSort;

            // Act
            var actualIsDescendingSort = winLossDetailParam.IsDescendingSort;

            // Assert
            Assert.Equal(expectedIsDescendingSort, actualIsDescendingSort);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("1,2")]
        public void UserSelectedProductIds_Always_ReturnsUserSelectedProductIdsFromConstructor(string userSelectedProductIds)
        {
            // Arrange
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();

            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                userSelectedProductIdList,
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = winLossDetailParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }

        [Fact]
        public void PageIndex_NotSupportPageIndex_ReturnsDefaultPageIndex()
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedPageIndex = DefaultVariablesTest.PageIndex;

            // Act
            var actualPageIndex = winLossDetailParam.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(1000)]
        [InlineData(2)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void PageIndex_SupportPageIndex_ReturnsSupportedPageIndex(int pageIndex)
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                "",
                1,
                pageIndex,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedPageIndex = pageIndex;

            // Act
            var actualPageIndex = winLossDetailParam.PageIndex;

            // Assert
            Assert.Equal(expectedPageIndex, actualPageIndex);
        }

        [Fact]
        public void PageSize_NotSupportPageSize_ReturnsDefaultPageSize()
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedPageSize = DefaultVariablesTest.PageSize;

            // Act
            var actualPageSize = winLossDetailParam.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(1000)]
        [InlineData(2)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void PageSize_SupportPageSize_ReturnsSupportedPageSize(int pageSize)
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                0,
                1,
                "",
                1,
                0,
                pageSize,
                DateTime.MinValue,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                string.Empty,
                false,
                false,
                string.Empty);
            var expectedPageSize = pageSize;

            // Act
            var actualPageSize = winLossDetailParam.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }

        [Fact]
        public void SortByPropertyName_Always_ReturnsSortByPropertyNameSetFromCtor()
        {
            // Arrange
            var winLossDetailParam = new WinLossDetailParam(
                1,
                1,
                string.Empty,
                1,
                0,
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                Enumerable.Empty<int>(),
                UserLevel.Super,
                "SortByPropertyName",
                false,
                false,
                string.Empty);

            // Act
            var actual = winLossDetailParam.SortByPropertyName;

            // Assert
            Assert.Equal("SortByPropertyName", actual);
        }
    }
}