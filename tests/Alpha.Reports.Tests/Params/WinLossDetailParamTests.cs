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
    using Reports.Services;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    public class WinLossDetailParamTests
    {
        public WinLossDetailParamTests()
        {
            DateTimeManager.Now = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var custId = 1;
            var winLossDetailParam = new WinLossDetailParam(custId, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actual = winLossDetailParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);
            var expectedDate = DefaultVariables.CurrentDate;

            // Act
            var actualDate = winLossDetailParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_HistoricReportAndNotSupportFromDate_ReturnsFirstDateOfThisMonth()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true);
            var expectedDate = DefaultVariables.CurrentDate.FirstDayOfMonth();

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, isHistoricReport);
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var winLossDetailParam = new WinLossDetailParam(
                    0,
                    0,
                    0,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    Enumerable.Empty<int>(),
                    reportLevel,
                    string.Empty,
                    false,
                    isHistoricReport);

            var expectedDate = DefaultVariables.CurrentDate;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime toDate = DefaultVariables.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, isHistoricReport);
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);
            var expectedReportLevel = reportLevel;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                string.Empty,
                isDescendingSort,
                false);
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, userSelectedProductIdList, reportLevel, string.Empty, false, false);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = winLossDetailParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }

        [Theory]
        [InlineData(4, "SuperNetWinLoss.WinLoss")]
        [InlineData(3, "MasterNetWinLoss.WinLoss")]
        [InlineData(2, "AgentNetWinLoss.WinLoss")]
        public void SortByPropertyName_NotSupportSortByPropertyName_ReturnsDefaultSortedPropertyByUserLevel(byte userLevel, string expectedSortByPropertyName)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(userLevel);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actualSortByPropertyName = winLossDetailParam.SortByPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Theory]
        [InlineData(3, "SuperNetWinLoss.WinLoss", "MasterNetWinLoss.WinLoss")]
        [InlineData(3, "SuperNetWinLoss.Comm", "MasterNetWinLoss.WinLoss")]
        [InlineData(3, "SuperNetWinLoss.Total", "MasterNetWinLoss.WinLoss")]
        [InlineData(2, "SuperNetWinLoss.WinLoss", "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "SuperNetWinLoss.Comm", "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "SuperNetWinLoss.Total", "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "MasterNetWinLoss.WinLoss", "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "MasterNetWinLoss.Comm", "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "MasterNetWinLoss.Total", "AgentNetWinLoss.WinLoss")]
        public void SortByPropertyName_SupportedSortByPropertyNameNotInRange_ReturnsDefaultSortedPropertyByUserLevel(
            byte userLevel,
            string notInRangeSortByPropertyName,
            string expectedSortByPropertyName)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(userLevel);

            var winLossDetailParam = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                notInRangeSortByPropertyName,
                false,
                false);

            // Act
            var actualSortByPropertyName = winLossDetailParam.SortByPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Theory]
        [InlineData(4, "SuperNetWinLoss.WinLoss")]
        [InlineData(4, "SuperNetWinLoss.Comm")]
        [InlineData(4, "SuperNetWinLoss.Total")]
        [InlineData(4, "MasterNetWinLoss.WinLoss")]
        [InlineData(4, "MasterNetWinLoss.Comm")]
        [InlineData(4, "MasterNetWinLoss.Total")]
        [InlineData(4, "AgentNetWinLoss.WinLoss")]
        [InlineData(4, "AgentNetWinLoss.Comm")]
        [InlineData(4, "AgentNetWinLoss.Total")]
        [InlineData(4, "CustId")]
        [InlineData(4, "CustName")]
        [InlineData(4, "Turnover")]
        [InlineData(4, "NetTurnover")]
        [InlineData(4, "GrossComm")]
        [InlineData(4, "NumberOfTickets")]
        [InlineData(4, "Company")]
        [InlineData(3, "MasterNetWinLoss.WinLoss")]
        [InlineData(3, "MasterNetWinLoss.Comm")]
        [InlineData(3, "MasterNetWinLoss.Total")]
        [InlineData(3, "AgentNetWinLoss.WinLoss")]
        [InlineData(3, "AgentNetWinLoss.Comm")]
        [InlineData(3, "AgentNetWinLoss.Total")]
        [InlineData(3, "CustId")]
        [InlineData(3, "CustName")]
        [InlineData(3, "Turnover")]
        [InlineData(3, "NetTurnover")]
        [InlineData(3, "GrossComm")]
        [InlineData(3, "NumberOfTickets")]
        [InlineData(3, "Company")]
        [InlineData(2, "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "AgentNetWinLoss.Comm")]
        [InlineData(2, "AgentNetWinLoss.Total")]
        [InlineData(2, "CustId")]
        [InlineData(2, "CustName")]
        [InlineData(2, "Turnover")]
        [InlineData(2, "NetTurnover")]
        [InlineData(2, "GrossComm")]
        [InlineData(2, "NumberOfTickets")]
        [InlineData(2, "Company")]
        public void SortByPropertyName_SupportedSortByPropertyNameInRange_ReturnsSupportedSortByPropertyName(
            byte userLevel,
            string inRangeSortByPropertyName)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(userLevel);

            var winLossDetailParam = new WinLossDetailParam(
                0,
                0,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                inRangeSortByPropertyName,
                false,
                false);
            var expectedSortByPropertyName = inRangeSortByPropertyName;

            // Act
            var actualSortByPropertyName = winLossDetailParam.SortByPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Fact]
        public void IsValid_IsHistoryReportAndFromDateLessThan3Years_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate.AddYears(-4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_IsHistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;
            DateTime toDate = DefaultVariables.CurrentDate.AddDays(-1);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public void IsValid_IsHistoryReportAndCrossMonthRange_ReturnsFalse(int monthDiff)
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;
            DateTime toDate = DefaultVariables.CurrentDate.AddMonths(monthDiff);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_IsHistoryReportAndValidRange_ReturnsTrue()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;
            DateTime toDate = DefaultVariables.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, true);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_NotHistoryReportAndFromDateLessThan3Months_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate.AddMonths(-4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_NotHistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;
            DateTime toDate = DefaultVariables.CurrentDate.AddDays(-1);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_NotHistoryReportAndValidDateRange_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;
            DateTime toDate = DefaultVariables.CurrentDate;

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);

            // Act
            var actualIsValid = winLossDetailParam.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void PageIndex_NotSupportPageIndex_ReturnsDefaultPageIndex()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);
            var expectedPageIndex = DefaultVariables.PageIndex;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var winLossDetailParam = new WinLossDetailParam(
                0,
                pageIndex,
                0,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                string.Empty,
                false,
                false);
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossDetailParam =
                new WinLossDetailParam(0, 0, 0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, string.Empty, false, false);
            var expectedPageSize = DefaultVariables.PageSize;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var winLossDetailParam = new WinLossDetailParam(
                0,
                0,
                pageSize,
                DateTime.MinValue,
                DateTime.MinValue,
                Enumerable.Empty<int>(),
                reportLevel,
                string.Empty,
                false,
                false);
            var expectedPageSize = pageSize;

            // Act
            var actualPageSize = winLossDetailParam.PageSize;

            // Assert
            Assert.Equal(expectedPageSize, actualPageSize);
        }
    }
}
