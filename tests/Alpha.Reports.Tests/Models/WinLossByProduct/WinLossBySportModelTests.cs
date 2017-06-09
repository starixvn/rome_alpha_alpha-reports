using System.Linq;
using Alpha.Reports.Models.WinlossByBetType;
using Alpha.Reports.Models.WinLossByProduct;
using Xunit;

namespace Alpha.Reports.Tests.Models.WinLossByProduct
{
    [Trait("Category", "WinLossByProduct")]
    public class WinLossBySportModelTests
    {
        [Theory]
        [InlineData(1, true)]
        [InlineData(23, false)]
        public void IsShowBetTypeDirectly_Always_ReturnsCorrectValue(int productId, bool expectedValue)
        {
            // Act
            var actualValue = WinLossBySportModel.IsShowBetTypeDirectly(productId);

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
