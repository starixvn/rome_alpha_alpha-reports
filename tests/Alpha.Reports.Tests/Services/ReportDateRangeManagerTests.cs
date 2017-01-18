namespace Alpha.Reports.Tests.Services
{
    using System;
    using Constants;
    using Reports.Services;
    using Xunit;

    public class ReportDateRangeManagerTests
    {
        [Fact]
        public void ValidateDateRange_HistoryReportAndFromDateLessThan3Years_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate.AddYears(-4),
                toDate: DateTime.Now,
                isHistoricReport: true);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_HistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddDays(-1),
                isHistoricReport: true);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public void ValidateDateRange_HistoryReportAndCrossMonthRange_ReturnsFalse(int monthDiff)
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddMonths(monthDiff),
                isHistoricReport: true);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_HistoryReportAndValidRange_ReturnsTrue()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate,
                isHistoricReport: true);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndFromDateLessThan3Months_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate.AddMonths(-4),
                toDate: DateTime.Now,
                isHistoricReport: false);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate.AddDays(-1),
                isHistoricReport: false);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NotHistoryReportAndValidDateRange_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange(
                DateRangeType.ThreeMonthCurrentOrThreeYearHistory,
                fromDate: DefaultVariables.CurrentDate,
                toDate: DefaultVariables.CurrentDate,
                isHistoricReport: false);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void ValidateDateRange_DateRageTypeIsNotSupported_ReturnsFalse()
        {
            // Arrange
            var dateRange = new DateRange((DateRangeType)10, DateTime.Now, DateTime.Now);

            // Act
            var isValid = ReportDateRangeManager.ValidateDateRange(dateRange);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void ValidateDateRange_NullParam_ThrowsArgumentNullException()
        {
            // Arrange
            DateRange dateRange = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => ReportDateRangeManager.ValidateDateRange(dateRange));
        }
    }
}