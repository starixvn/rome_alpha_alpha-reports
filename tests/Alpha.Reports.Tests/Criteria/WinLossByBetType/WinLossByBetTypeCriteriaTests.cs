namespace Alpha.Reports.Tests.Criteria.WinLossByBetType
{
    using System;
    using Reports.Criteria.WinLossByBetTypeCriteria;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLossByBettype_Member")]
        [InlineData(3, "Age_Rpt_WinLossByBettype_Agent")]
        [InlineData(4, "Age_Rpt_WinLossByBettype_Master")]
        public void WinLossByBetTypeCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossByBetTypeCriteria = new WinLossByBetTypeCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, userLevel);

            // Act
            var actualSettingKey = winLossByBetTypeCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
