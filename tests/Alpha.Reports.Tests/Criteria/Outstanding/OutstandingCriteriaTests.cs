namespace Alpha.Reports.Tests.Criteria.WinLossDetail
{
    using Reports.Criteria.Outstanding;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "Outstanding")]
    public class OutstandingCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_Outstanding_Member")]
        [InlineData(3, "Age_Rpt_Outstanding_Agent")]
        [InlineData(4, "Age_Rpt_Outstanding_Master")]
        public void OutstandingCriteria_WithCorrectCustLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new OutstandingCriteria(custLevel, 0, null);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(4, true)]
        public void OutstandingCriteria_WithCorrectCustLevel_ReturnsValid(byte custLevelId, bool expectedValue)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new OutstandingCriteria(custLevel, 0, null);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedValue, isValid);
        }
    }
}
