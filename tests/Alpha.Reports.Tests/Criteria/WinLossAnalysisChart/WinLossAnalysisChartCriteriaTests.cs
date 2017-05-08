namespace Alpha.Reports.Tests.Criteria.WinLossAnalysisChart
{
    using Reports.Criteria.WinLossAnalysisChart;
    using Reports.Enumerations;
    using Reports.Models;
    using System;
    using Xunit;

    [Trait("Category", "WinLossAnalysisChart")]
    public class WinLossAnalysisChartCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Chart_WinlossAnalysis_Member")]
        [InlineData(3, "Age_Chart_WinlossAnalysis_Agent")]
        [InlineData(4, "Age_Chart_WinlossAnalysis_Master")]
        public void WinLossAnalysisChartCriteria_WithCorrectUserLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var userLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var winLossAnalysisChartCriteria = new WinLossAnalysisChartCriteria(0, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, null, userLevel);

            // Act
            var actualSettingKey = winLossAnalysisChartCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}