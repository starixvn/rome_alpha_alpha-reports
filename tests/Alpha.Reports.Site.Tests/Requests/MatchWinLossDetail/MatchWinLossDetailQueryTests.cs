namespace Alpha.Reports.Site.Tests.Requests.MatchWinLossDetail
{
    using Fixtures;
    using Site.Requests.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("UserCollection")]
    public class MatchWinLossDetailQueryTests
    {
        private readonly MatchWinLossDetailQuery matchWinLossDetailQuery;

        public MatchWinLossDetailQueryTests(UserFixture userFixture)
        {
            matchWinLossDetailQuery = new MatchWinLossDetailQuery(userFixture.User);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsSetDefaultStackMode_NullOfWhiteSpaceUserSelectedProductIds_ReturnsTrue(string userSelectedProductIds)
        {
            // Arrange
            matchWinLossDetailQuery.UserSelectedProductIds = userSelectedProductIds;

            // Act
            var actualIsSetDefaultStackMode = matchWinLossDetailQuery.IsSetDefaultStackMode;

            // Assert
            Assert.True(actualIsSetDefaultStackMode);
        }

        [Fact]
        public void IsSetDefaultStackMode_NotNullOfWhiteSpaceUserSelectedProductIds_ReturnsFalse()
        {
            // Arrange
            matchWinLossDetailQuery.UserSelectedProductIds = "1,2,3";

            // Act
            var actualIsSetDefaultStackMode = matchWinLossDetailQuery.IsSetDefaultStackMode;

            // Assert
            Assert.False(actualIsSetDefaultStackMode);
        }
    }
}
