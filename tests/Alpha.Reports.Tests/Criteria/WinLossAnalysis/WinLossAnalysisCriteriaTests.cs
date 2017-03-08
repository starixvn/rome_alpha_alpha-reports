namespace Alpha.Reports.Tests.Criteria.WinLossAnalysis
{
    using System;
    using Reports.Criteria.WinLossAnalysis;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public class WinLossAnalysisCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_WinlossAnalysis_Member")]
        [InlineData(3, "Age_Rpt_WinlossAnalysis_Agent")]
        [InlineData(4, "Age_Rpt_WinlossAnalysis_Master")]
        public void WinLossAnalysisCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossCriteria = new WinLossAnalysisCriteria(0, 1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, userLevel);

            // Act
            var actualSettingKey = winLossCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
