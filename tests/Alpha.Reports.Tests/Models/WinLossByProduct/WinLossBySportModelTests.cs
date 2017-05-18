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
        [InlineData(1, "Sportsbook", true)]
        [InlineData(23, "NG2", false)]
        public void IsShowBetTypeDirectly_Always_ReturnsCorrectValue(int sportId, string sportName, bool expectedValue)
        {
            // Arrange
            var winLossByProductModel = new WinLossBySportModel(
                   sportId: sportId,
                   sportName: sportName,
                   winLossByBetTypeData: Enumerable.Empty<WinLossByBetTypeModel>());

            // Act
            var actualValue = winLossByProductModel.IsShowBetTypeDirectly();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
