namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using Fixtures;
    using Reports.Criteria.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class MemberPlayForFunCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public MemberPlayForFunCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<MemberPlayForFunCriteria>();
            var expectedSettingKey = "Age_Rpt_PlayForFun_Member";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
