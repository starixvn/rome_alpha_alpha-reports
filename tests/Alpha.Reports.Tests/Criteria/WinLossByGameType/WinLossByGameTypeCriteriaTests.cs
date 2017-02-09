namespace Alpha.Reports.Tests.Criteria.WinLossByGameType
{
    using System;
    using Reports.Criteria.WinLossByGameType;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLossByGameType")]
    public class WinLossByGameTypeCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLossByGameType_Member")]
        [InlineData(3, "Age_Rpt_WinLossByGameType_Agent")]
        [InlineData(4, "Age_Rpt_WinLossByGameType_Master")]
        public void WinLossByGameTypeCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossByGameTypeCriteria = new WinLossByGameTypeCriteria(0, 0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, userLevel);

            // Act
            var actualSettingKey = winLossByGameTypeCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
