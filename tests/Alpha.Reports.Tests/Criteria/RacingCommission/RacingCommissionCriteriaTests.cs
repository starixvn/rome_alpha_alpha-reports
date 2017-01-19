namespace Alpha.Reports.Tests.Criteria.RacingCommission
{
    using System;
    using Reports.Criteria.RacingCommission;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "RacingCommission")]
    public class RacingCommissionCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_RacingCommission_Member")]
        [InlineData(3, "Age_Rpt_RacingCommission_Agent")]
        [InlineData(4, "Age_Rpt_RacingCommission_Master")]
        public void RacingCommissionCriteria_WithCorrectCustLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new RacingCommissionCriteria(custLevel, 0, new DateTime(), new DateTime());

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(4, true)]
        public void RacingCommissionCriteria_WithCorrectCustLevel_ReturnsValid(byte custLevelId, bool expectedValue)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new RacingCommissionCriteria(custLevel, 0, new DateTime(), new DateTime());

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, isValid);
        }
    }
}
