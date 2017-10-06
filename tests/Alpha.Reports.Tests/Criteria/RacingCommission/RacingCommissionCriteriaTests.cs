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
        [InlineData(2, "Age_Rpt_CommByBettype_HR_Member")]
        [InlineData(3, "Age_Rpt_CommByBettype_HR_Agent")]
        [InlineData(4, "Age_Rpt_CommByBettype_HR_Master")]
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

        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new RacingCommissionCriteria(UserLevel.Agent, 1, DateTime.MinValue, DateTime.MinValue);

            // Assert
            Assert.True(
                criteria.CustId == 1
                && criteria.FromDate == DateTime.MinValue
                && criteria.ToDate == DateTime.MinValue);
        }
    }
}
