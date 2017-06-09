namespace Alpha.Reports.Tests.Criteria.WinLossAnalysisChart
{
    using Constants;
    using Reports.Criteria.WinLossAnalysisChart;
    using Reports.Enumerations;
    using Reports.Models;
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void IsValid_MonthRangeIsLessThan3Months_ReturnFalse()
        {
            // Arrange
            var userSelectedProductIdList = new List<int> { 1, 2 };
            var fromDate = DefaultVariables.CurrentDate.AddMonths(-1);
            var toDate = DefaultVariables.CurrentDate;
            var criteria = new WinLossAnalysisChartCriteria(1, fromDate, toDate, DateTime.Now, DateTime.Now, userSelectedProductIdList, UserLevel.Super);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_MonthRangeIsEqual3Months_ReturnTrue()
        {
            // Arrange
            var userSelectedProductIdList = new List<int> { 1, 2 };
            var fromDate = DefaultVariables.CurrentDate.AddMonths(-2);
            var toDate = DefaultVariables.CurrentDate;
            var criteria = new WinLossAnalysisChartCriteria(1, fromDate, toDate, DateTime.Now, DateTime.Now, userSelectedProductIdList, UserLevel.Super);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_MonthRangeIsMoreThan3Months_ReturnTrue()
        {
            // Arrange
            var userSelectedProductIdList = new List<int> { 1, 2 };
            var fromDate = DefaultVariables.CurrentDate.AddMonths(-3);
            var toDate = DefaultVariables.CurrentDate;
            var criteria = new WinLossAnalysisChartCriteria(1, fromDate, toDate, DateTime.Now, DateTime.Now, userSelectedProductIdList, UserLevel.Super);

            // Act
            var isValid = criteria.IsValid();

            // Assert
            Assert.True(isValid);
        }
    }
}