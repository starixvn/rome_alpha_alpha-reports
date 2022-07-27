namespace Alpha.Reports.Tests.Features.BetCreditTracking.BetCreditHistory.Criteria
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Criteria;
    using Xunit;

    [Trait("Category", "BetCreditHistory")]
    public class GetLogActionsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetLogActionsCriteria();
            var expectedSettingKey = "Acc_UserLog_LogActionSel";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetLogActionsCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
