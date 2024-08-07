namespace Alpha.Reports.Tests.Features.BetCreditTracking.MoveStatementHistory.Criteria
{
    using System;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Params;
    using Xunit;

    [Trait("Category", "MoveStatementHistory")]
    public class MoveStatementHistoryCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", new DateTime(2022, 1, 1), new DateTime(2022, 1, 2), 1, DateTime.MinValue, 1, 100, true);

            // Act
            var criteria = new MoveStatementHistoryCriteria(
                param.Username, param.FromDate, param.ToDate, param.ViewerId, param.PageIndex, param.PageSize, param.IsVariance);

            // Assert
            Assert.True(
                param.Username == criteria.Username
                && param.FromDate == criteria.FromDate
                && param.ToDate == criteria.ToDate
                && param.ViewerId == criteria.LoggedId
                && param.PageIndex == criteria.PageIndex
                && param.PageSize == criteria.PageSize
                );
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100, true);
            var criteria = new MoveStatementHistoryCriteria(
                param.Username, param.FromDate, param.ToDate, param.ViewerId, param.PageIndex, param.PageSize, param.IsVariance);
            var expectedSettingKey = "Age_Rpt_StatementMovement";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100, true);
            var criteria = new MoveStatementHistoryCriteria(
                param.Username, param.FromDate, param.ToDate, param.ViewerId, param.PageIndex, param.PageSize, param.IsVariance);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var param = new MoveStatementHistoryParam("member1", DateTime.Today, DateTime.Today.AddDays(-1), 1, DateTime.MinValue, 1, 100, true);
            var criteria = new MoveStatementHistoryCriteria(
                param.Username, param.FromDate, param.ToDate, param.ViewerId, param.PageIndex, param.PageSize, param.IsVariance);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }
    }
}
