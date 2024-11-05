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
            var winLossCriteria = new WinLossMemberCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, string.Empty);

            // Act
            var actualSettingKey = winLossCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
