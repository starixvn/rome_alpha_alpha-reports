namespace Alpha.Reports.Tests.Criteria
{
    using Alpha.Reports.Criteria;
    using Xunit;

    public class GetProductsDetailCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetProductsDetailCriteria();
            var expectedSettingKey = "Age_Rpt_GetProductDetail";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsTrue()
        {
            // Arrange
            var criteria = new GetProductsDetailCriteria();

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
