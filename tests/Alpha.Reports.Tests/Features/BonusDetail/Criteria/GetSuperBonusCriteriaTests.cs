namespace Alpha.Reports.Tests.Features.BonusDetail.Criteria
{
    using System;
    using Alpha.Reports.Features.BonusDetail.Criteria;
    using Alpha.Reports.Features.BonusDetail.Params;
    using Alpha.Reports.Models;
    using Xunit;

    [Trait("Category", "BonusDetail")]
    public class GetSuperBonusCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Super, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetSuperBonusCriteria(param);
            var expectedSettingKey = "Age_Rpt_BonusDetail_Super";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-8, false)]
        [InlineData(0, false)]
        [InlineData(8, true)]
        public void IsValid_SuperId_ReturnsExpectedValue(int superId, bool expected)
        {
            // Arrange
            var param = new BonusDetailParam(superId, UserLevel.Agent, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetAgentBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_FromDateLessThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Super, null, DateTime.Today.AddDays(-1), DateTime.Today, DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetSuperBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsValid_FromDateGreaterThanToDate_ReturnsTrue()
        {
            // Arrange
            var param = new BonusDetailParam(8, UserLevel.Super, null, DateTime.Today, DateTime.Today.AddDays(-1), DateTime.MinValue, null, null, 1, 500);
            var criteria = new GetSuperBonusCriteria(param);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }
    }
}
