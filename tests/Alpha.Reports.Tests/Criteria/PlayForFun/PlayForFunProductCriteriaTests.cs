namespace Alpha.Reports.Tests.Criteria.PlayForFun
{
    using Fixtures;
    using Reports.Criteria.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("AnyValueCreatorCollection")]
    public class PlayForFunProductCriteriaTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public PlayForFunProductCriteriaTests(AnyValueCreatorFixture anyValueCreatorFixture)
        {
            this.anyValueCreatorFixture = anyValueCreatorFixture;
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsCorrectSettingKey()
        {
            // Arrange
            var criteria = anyValueCreatorFixture.Create<PlayForFunProductCriteria>();
            var expectedSettingKey = "Age_Rpt_PlayForFun_ProductSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnFalse(int custId)
        {
            // Arrange
            var criteria = new PlayForFunProductCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Fact]
        public void IsValid_ValidCriteria_ReturnTrue()
        {
            // Arrange
            var criteria = new PlayForFunProductCriteria(1);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
