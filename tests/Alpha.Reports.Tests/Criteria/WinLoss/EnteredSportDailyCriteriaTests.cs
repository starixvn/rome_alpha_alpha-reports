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
            var enteredSportDailyCriteria = new DailyEnteredSportCriteria(false);

            // Act
            var actualSettingKey = enteredSportDailyCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var enteredSportDailyCriteria = new DailyEnteredSportCriteria(false);

            // Act
            var actualIsValid = enteredSportDailyCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
