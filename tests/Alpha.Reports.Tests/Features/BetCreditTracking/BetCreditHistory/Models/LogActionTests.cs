namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory.Models
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class LogActionTests
    {
        [Fact]
        public void Constructor_Default_AssignsCorrectProperties()
        {
            // Arrange & Act
            var actual = new LogAction();

            // Assert
            Assert.True(
                actual.Id == 0 &&
                actual.ActionTypeId == 0 &&
                actual.ChineseTraditionalName == null &&
                actual.ChineseSimplifiedName == null &&
                actual.EnglishName == null &&
                actual.VietnameseName == null &&
                actual.JapaneseName == null &&
                actual.KoreanName == null &&
                actual.ThaiName == null &&
                actual.IsInternal == false &&
                actual.SPName == null);
        }
    }
}
