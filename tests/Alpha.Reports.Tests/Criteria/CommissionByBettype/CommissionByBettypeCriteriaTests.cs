namespace Alpha.Reports.Tests.Criteria.CommissionByBettype
{
    using System;
    using Reports.Criteria.CommissionByBettype;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "CommissionByBettype")]
    public class CommissionByBettypeCriteriaTests
    {
        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(4, true)]
        public void ValidState_If_CustLevel_IsNot_MemberLevel(byte custLevelId, bool expectedValue)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new CommissionByBettypeMemberCriteria(0, new DateTime(), new DateTime(), new DateTime());

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, isValid);
        }

        [Theory]
        [InlineData(4, "Age_Rpt_CommByBettype_Master")]
        [InlineData(3, "Age_Rpt_CommByBettype_Agent")]
        [InlineData(2, "Age_Rpt_CommByBettype_Member")]
        public void With_ValidCustLevel_Returns_ValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var criteria = new CommissionByBettypeMemberCriteria(0, new DateTime(), new DateTime(), new DateTime());

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new CommissionByBettypeMemberCriteria(1, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            // Assert
            Assert.True(
                criteria.CustId == 1
                && criteria.FromDate == DateTime.MinValue
                && criteria.ToDate == DateTime.MinValue
                && criteria.MoveBetsDay == DateTime.MinValue);
        }
    }
}
