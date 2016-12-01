namespace Alpha.Reports.Tests.Criteria
{
    using Reports.Criteria;
    using Xunit;

    public class GetBalanceUpDayAndMoveBetsDayCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getBalanceUpDayAndMoveBetsDayCriteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
            var expectedSettingKey = "Age_SelBalanceUpDay";

            // Act
            var actualSettingKey = getBalanceUpDayAndMoveBetsDayCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsValid()
        {
            // Arrange
            var getBalanceUpDayAndMoveBetsDayCriteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
            var expectedIsValid = true;

            // Act
            var actualIsValid = getBalanceUpDayAndMoveBetsDayCriteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }
    }
}
