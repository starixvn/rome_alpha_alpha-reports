namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using Fixtures;
    using Reports.Criteria.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class AgentPlayForFunCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public AgentPlayForFunCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<AgentPlayForFunCriteria>();
            var expectedSettingKey = "Age_Rpt_PlayForFun_Agent";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
