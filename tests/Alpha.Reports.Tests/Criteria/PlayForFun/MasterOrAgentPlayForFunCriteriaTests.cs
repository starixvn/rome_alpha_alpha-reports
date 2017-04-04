namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using System;
    using Reports.Criteria.PlayForFun;
    using Reports.Models;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    public class MasterOrAgentPlayForFunCriteriaTests
    {
        [Fact]
        public void GetSettingKey_MasterLevel_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new MasterOrAgentPlayForFunCriteria(0, DateTime.Now, DateTime.Now, 0, 0, UserLevel.Super);
            var expectedSettingKey = "Age_Rpt_PlayForFun_Master";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void GetSettingKey_AgentLevel_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = new MasterOrAgentPlayForFunCriteria(0, DateTime.Now, DateTime.Now, 0, 0, UserLevel.Master);
            var expectedSettingKey = "Age_Rpt_PlayForFun_Agent";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
