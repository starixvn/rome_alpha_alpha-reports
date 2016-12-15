namespace Alpha.Reports.Tests.Criteria.WinLoss
{
    using Reports.Criteria.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    public class EnteredSportDailyCriteriaTests
    {
        [Fact]
        public void EnteredSportDailyCriteria_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var expectedSettingKey = "Age_Rpt_MatchSelResultEntered";
            var winLossCriteria = new EnteredSportDailyCriteria();

            // Act
            var actualSettingKey = winLossCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }
    }
}
