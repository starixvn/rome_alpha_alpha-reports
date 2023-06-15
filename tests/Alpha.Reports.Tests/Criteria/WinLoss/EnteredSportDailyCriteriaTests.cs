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
            var enteredSportDailyCriteria = new DailyEnteredSportCriteria();

            // Act
            var actualSettingKey = enteredSportDailyCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var enteredSportDailyCriteria = new DailyEnteredSportCriteria();

            // Act
            var actualIsValid = enteredSportDailyCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }

        [Fact]
        public void IncludeTestData_Always_ReturnsIncludeTestDataSetFromConstructor()
        {
            // Arrange
            var enteredSportDailyCriteria = new DailyEnteredSportCriteria();

            // Act
            var actualIncludeTestData = enteredSportDailyCriteria.IncludeTestData;

            // Assert
            Assert.Equal(false, actualIncludeTestData);
        }
    }
}
