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

    [Trait("Category", "WinLoss")]
    public class WinLossParamTests
    {
        public WinLossParamTests()
        {
            DateTimeManager.Now = DefaultVariables.CurrentDate;
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdSetFromCtor()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var custId = 1;
            var winLossParam = new WinLossParam(custId, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actual = winLossParam.CustId;

            // Assert
            Assert.Equal(custId, actual);
        }

        [Fact]
        public void FromDate_CurrentReportAndNotSupportFromDate_ReturnsCurrentDate()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, false);

            var expectedDate = DefaultVariables.CurrentDate;

            // Act
            var actualDate = winLossParam.FromDate;

            // Assert
            Assert.Equal(expectedDate, actualDate);
        }

        [Fact]
        public void FromDate_HistoricReportAndNotSupportFromDate_ReturnsFirstDateOfThisMonth()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.Now, Enumerable.Empty<int>(), reportLevel, true);

            var expectedDate = DefaultVariables.CurrentDate.FirstDayOfMonth();

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate;

            var winLossParam = new WinLossParam(0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, isHistoricReport);
           
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, isHistoricReport);

            var expectedDate = DefaultVariables.CurrentDate;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime toDate = DefaultVariables.CurrentDate;

            var winLossParam = new WinLossParam(0, DateTime.MinValue, toDate, Enumerable.Empty<int>(), reportLevel, isHistoricReport);
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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);

            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, Enumerable.Empty<int>(), reportLevel, false);
            var expectedReportLevel = reportLevel;

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
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            var userSelectedProductIdList = userSelectedProductIds.Split(',').Select(id => Converter.ToInt(id)).ToList();

            var winLossParam = new WinLossParam(0, DateTime.MinValue, DateTime.MinValue, userSelectedProductIdList, reportLevel, false);
            var expectedUserSelectedProductIds = userSelectedProductIdList;

            // Act
            var actualUserSelectedProductIds = winLossParam.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }

        [Fact]
        public void IsValid_IsHistoryReportAndFromDateLessThan3Years_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate.AddYears(-4);

            var winLossParam = new WinLossParam(0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

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

            var winLossParam = new WinLossParam(0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

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

            var winLossParam = new WinLossParam(0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, true);

            // Act
            var actualIsValid = winLossParam.IsValid();

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

            var winLossParam = new WinLossParam(0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IsValid_NotHistoryReportAndFromDateLessThan3Months_ReturnsFalse()
        {
            // Arrange
            UserLevel reportLevel = Enumeration.FromValue<UserLevel>(4);
            DateTime fromDate = DefaultVariables.CurrentDate.AddMonths(-4);

            var winLossParam = new WinLossParam(0, fromDate, DateTime.Now, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

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

            var winLossParam = new WinLossParam(0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

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

            var winLossParam = new WinLossParam(0, fromDate, toDate, Enumerable.Empty<int>(), reportLevel, false);

            // Act
            var actualIsValid = winLossParam.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
