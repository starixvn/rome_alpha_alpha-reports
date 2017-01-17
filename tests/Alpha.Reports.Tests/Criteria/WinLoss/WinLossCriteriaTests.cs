namespace Alpha.Reports.Tests.Criteria.WinLoss
{
    using System;
    using Reports.Criteria.WinLoss;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class WinLossCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinLoss_Member")]
        [InlineData(3, "Age_Rpt_WinLoss_Agent")]
        [InlineData(4, "Age_Rpt_WinLoss_Master")]
        public void WinLossCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossCriteria = new WinLossCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, userLevel);

            // Act
            var actualSettingKey = winLossCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
