using Alpha.Reports.Models;
using Alpha.Reports.Site.Requests.WinLossByProduct;
using Alpha.Reports.Site.Tests.Fixtures;
using Xunit;

namespace Alpha.Reports.Site.Tests.Requests.WinLossByProduct
{
    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductQueryTests
    {
        private readonly IUser user;

        public WinLossByProductQueryTests(UserFixture userFixture)
        {
            user = userFixture.User;
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void IsSetDefaultStackMode_NullOrWhiteSpaceProductIds_ReturnsTrue(string productIds)
        {
            // Arrange
            var query = new WinLossByProductQuery(user)
            {
                UserSelectedProductIds = productIds
            };

            // Act
            var actualIsSetDefaultStackMode = query.IsSetDefaultStackMode;

            // Assert
            Assert.True(actualIsSetDefaultStackMode);
        }

        [Fact]
        public void IsSetDefaultStackMode_HasProductIds_ReturnsFalse()
        {
            // Arrange
            var query = new WinLossByProductQuery(user)
            {
                UserSelectedProductIds = "1"
            };

            // Act
            var actualIsSetDefaultStackMode = query.IsSetDefaultStackMode;

            // Assert
            Assert.False(actualIsSetDefaultStackMode);
        }
    }
}
