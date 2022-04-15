namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory.Criteria
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using System;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class BetCreditHistoryCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, new DateTime(2022, 1, 1), new DateTime(2022, 1, 2), 1, DateTime.MinValue, 1, 100);

            // Act
            var criteria = new BetCreditHistoryCriteria(param);

            // Assert
            Assert.True(
                param.Member == criteria.Username
                && param.RefNo == criteria.RefNo
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
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1, 100);
            var criteria = new BetCreditHistoryCriteria(param);
            var expectedSettingKey = "Age_Rpt_BetCreditHistory";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, 1 , 100);
            var criteria = new BetCreditHistoryCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today, DateTime.Today.AddDays(-1), 1, DateTime.MinValue, 1, 100);
            var criteria = new BetCreditHistoryCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }
    }
}
