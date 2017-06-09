namespace Alpha.Reports.Site.Tests.Requests.WinLossAnalysisChart
{
    using Constants;
    using Fixtures;
    using Reports.Extensions;
    using Site.Requests.WinLossAnalysisChart;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    [Collection("UserCollection")]
    public class WinLossAnalysisChartQueryTests
    {
        private readonly UserFixture userFixture;

        public WinLossAnalysisChartQueryTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
        }

        [Fact]
        public void Constructor_ToDayIsEndOfMonth_SetFromDateIsFirstDayOfLast2Months()
        {
            // Arrange
            SystemTime.Today = DefaultVariables.CurrentDate.LastDayOfMonth();
            var expectedFromDate = SystemTime.Today.AddMonths(-2).FirstDayOfMonth();
            var query = new WinLossAnalysisChartQuery(userFixture.User);

            // Act
            var acutalFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, acutalFromDate);
        }

        [Fact]
        public void Constructor_ToDayIsEndOfMonth_SetToDateIsToDay()
        {
            // Arrange
            SystemTime.Today = DefaultVariables.CurrentDate.LastDayOfMonth();
            var expectedToDate = SystemTime.Today;
            var query = new WinLossAnalysisChartQuery(userFixture.User);

            // Act
            var acutalToDate = query.ToDate;

            // Assert
            Assert.Equal(expectedToDate, acutalToDate);
        }

        [Fact]
        public void Constructor_ToDayIsNotEndOfMonth_SetFromDateIsFirstDayOfLast3Months()
        {
            // Arrange
            SystemTime.Today = DefaultVariables.CurrentDate;
            var expectedFromDate = SystemTime.Today.AddMonths(-3).FirstDayOfMonth();
            var query = new WinLossAnalysisChartQuery(userFixture.User);

            // Act
            var acutalFromDate = query.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, acutalFromDate);
        }

        [Fact]
        public void Constructor_ToDayIsNotEndOfMonth_SetToDateIsEndDayOfPreviousMonth()
        {
            SystemTime.Today = DefaultVariables.CurrentDate;
            var expectedToDate = SystemTime.Today.AddMonths(-1).LastDayOfMonth();
            var query = new WinLossAnalysisChartQuery(userFixture.User);

            // Act
            var acutalToDate = query.ToDate;

            // Assert
            Assert.Equal(expectedToDate, acutalToDate);
        }
    }
}