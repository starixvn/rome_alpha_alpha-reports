namespace Alpha.Reports.Tests.Criteria.ProgressiveContribution
{
    using System;
    using System.Linq;
    using Reports.Criteria.ProgressiveContribution;
    using Reports.Enumerations;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "ProgressiveGamesContribution")]
    public class ProgressiveContributionCriteriaTests
    {
        [Theory]
        [InlineData(2, "Age_Rpt_ProgressiveGamesContribution_Member")]
        [InlineData(3, "Age_Rpt_ProgressiveGamesContribution_Agent")]
        [InlineData(4, "Age_Rpt_ProgressiveGamesContribution_Master")]
        public void ProgressiveContributionCriteria_WithCorrectCustLevel_ReturnsValidSettingKey(byte custLevelId, string expectedSettingKey)
        {
            // Arrange
            var custLevel = Enumeration.FromValue<UserLevel>(custLevelId);
            var criteria = new ProgressiveContributionCriteria(1, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), custLevel);

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}