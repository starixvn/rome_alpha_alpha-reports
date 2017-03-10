namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using Fixtures;
    using Reports.Criteria.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class MasterPlayForFunCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MasterPlayForFunCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<MasterPlayForFunCriteria>();
            var expectedSettingKey = "Age_Rpt_PlayForFun_Master";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
