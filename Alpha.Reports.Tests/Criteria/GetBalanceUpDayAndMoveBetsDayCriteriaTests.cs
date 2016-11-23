namespace Alpha.Reports.Tests.Criteria
{
    using NUnit.Framework;
    using Reports.Criteria;

    [TestFixture]
    public class GetBalanceUpDayAndMoveBetsDayCriteriaTests
    {
        [Test]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getBalanceUpDayAndMoveBetsDayCriteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
            var expectedSettingKey = "Age_SelBalanceUpDay";

            // Act
            var actualSettingKey = getBalanceUpDayAndMoveBetsDayCriteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expectedSettingKey, actualSettingKey);
        }

        [Test]
        public void IsValid_Always_ReturnsValid()
        {
            // Arrange
            var getBalanceUpDayAndMoveBetsDayCriteria = new GetBalanceUpDayAndMoveBetsDayCriteria();
            var expectedIsValid = true;

            // Act
            var actualIsValid = getBalanceUpDayAndMoveBetsDayCriteria.IsValid();

            // Assert
            Assert.AreEqual(expectedIsValid, actualIsValid);
        }
    }
}
