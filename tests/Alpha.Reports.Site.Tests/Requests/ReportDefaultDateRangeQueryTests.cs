namespace Alpha.Reports.Site.Tests.Requests
{
    using System;
    using Constants;
    using Fixtures;
    using Reports.Extensions;
    using Reports.Models;
    using Site.Requests;
    using Utils;
    using Xunit;

    [Trait("Category", "Site.Request")]
    public class ReportDefaultDateRangeQueryTests
    {
        private readonly IUser user;

        public ReportDefaultDateRangeQueryTests()
        {
            user = (new UserFixture()).User;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
        }

        [Fact]
        public void IsValid_HistoricReportAndFromDateLessThan3Years_ReturnsFalse()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;
            query.FromDate = DefaultVariablesTest.CurrentDate.AddYears(-4);

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_HistoricReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate.AddDays(-1);

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(12)]
        public void IsValid_HistoricReportAndFromDateHasDifferenceMonthWithToDate_ReturnsFalse(int monthDiff)
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate.AddMonths(monthDiff);

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_HistoricReportAndSameDate_ReturnsTrue()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate;

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_NotHistoricReportAndFromDateLessThan3Months_ReturnsFalse()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = false;
            query.FromDate = DefaultVariablesTest.CurrentDate.AddMonths(-4);
            query.ToDate = DefaultVariablesTest.CurrentDate;

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_NotHistoricReportAndToDateLessThanFromDate_ReturnsFalse()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = false;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate.AddDays(-1);

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_NotHistoricReportAndValidDateRange_ReturnsFalse()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = false;
            query.FromDate = DefaultVariablesTest.CurrentDate;
            query.ToDate = DefaultVariablesTest.CurrentDate;

            // Act
            var isValid = query.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void MinDate_HistoricReport_ReturnFirstDateOfPrevious3Years()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(DefaultVariablesTest.CurrentDate.AddYears(-3).FirstDayOfYear(), actualMinDate);
        }

        [Fact]
        public void MinDate_ThreeMonthCurrentOrThreeYearHistory_CurrentReport_ReturnFirstDateOfPrevious3Months()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = false;

            // Act
            var actualMinDate = query.MinDate;

            // Assert
            Assert.Equal(DefaultVariablesTest.CurrentDate.AddMonths(-3).FirstDayOfMonth(), actualMinDate);
        }

        [Fact]
        public void IsValid_HistoricReportAndFromDateIsMinValue_SetFirstDayOfThisMonthForFromDate()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = true;

            // Act
            query.IsValid();
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(DefaultVariablesTest.CurrentDate.FirstDayOfMonth(), actualFromDate);
        }

        [Fact]
        public void IsValid_NotHistoricReportAndFromDateIsMinValue_SetTodayForFromDate()
        {
            // Arrange
            var query = new ReportDefaultDateRangeQuery(user);
            query.IsHistoricReport = false;

            // Act
            query.IsValid();
            var actualFromDate = query.FromDate;

            // Assert
            Assert.Equal(DefaultVariablesTest.CurrentDate, actualFromDate);
        }

        [Fact]
        public void Constructor_Always_SetDefaultValueForFromAndToDate()
        {
            // Act
            var query = new ReportDefaultDateRangeQuery(user);

            // Assert
            Assert.True(query.FromDate == DateTime.MinValue && query.ToDate == DefaultVariablesTest.CurrentDate);
        }
    }
}