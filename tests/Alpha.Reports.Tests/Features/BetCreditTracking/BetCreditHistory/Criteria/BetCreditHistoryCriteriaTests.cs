namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory.Criteria
{
    using System;
    using Alpha.Reports.Constants;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Params;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class BetCreditHistoryCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignCorrectProperties()
        {
            // Arrange & Act
            var criteria = new BetCreditHistoryCriteria(
                "supertest1",
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 2),
                123456,
                1,
                1,
                100);

            // Assert
            Assert.True(
                "supertest1" == criteria.Username
                && 123456 == criteria.RefNo
                && new DateTime(2022, 1, 1) == criteria.FromDate
                && new DateTime(2022, 1, 2) == criteria.ToDate
                && 1 == criteria.LoggedId
                && 1 == criteria.PageIndex
                && 100 == criteria.PageSize);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1, 100);
            var criteria = new BetCreditHistoryCriteria(
                param.Member, param.FromDate, param.ToDate, param.RefNo, param.ViewerId, param.PageIndex, param.PageSize);
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
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today.AddDays(-1), DateTime.Today, 1, DateTime.MinValue, LanguageCode.English, 1 , 100);
            var criteria = new BetCreditHistoryCriteria(
                param.Member, param.FromDate, param.ToDate, param.RefNo, param.ViewerId, param.PageIndex, param.PageSize);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsFalse()
        {
            // Arrange
            var param = new BetCreditHistoryParam("supertest1", 123456, DateTime.Today, DateTime.Today.AddDays(-1), 1, DateTime.MinValue, LanguageCode.English, 1, 100);
            var criteria = new BetCreditHistoryCriteria(
                param.Member, param.FromDate, param.ToDate, param.RefNo, param.ViewerId, param.PageIndex, param.PageSize);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }
    }
}
