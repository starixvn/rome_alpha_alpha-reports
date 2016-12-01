namespace Alpha.Reports.Tests.Criteria
{
    using Reports.Criteria;
    using Xunit;

    public class GetProductsCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var getProductsCriteria = new GetProductsCriteria();
            var expectedSettingKey = "Age_Rpt_GetProductList";

            // Act
            var actualSettingKey = getProductsCriteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue(int custId)
        {
            // Arrange
            var getProductsCriteria = new GetProductsCriteria();

            // Act
            var actualIsValid = getProductsCriteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
