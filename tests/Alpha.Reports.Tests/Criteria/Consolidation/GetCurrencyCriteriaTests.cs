namespace Alpha.Reports.Tests.Criteria.Consolidation
{
    using Reports.Criteria.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class GetCurrencyCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetCurrencyCriteria(custId: 0);
            var expectedSettingKey = "Age_Rpt_ConsolidatedGroupSelCurrency";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public void IsValid_InvalidCustId_ReturnsFalse(int custId)
        {
            // Arrange
            var criteria = new GetCurrencyCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.False(actualIsValid);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void IsValid_ValidCustId_ReturnsTrue(int custId)
        {
            // Arrange
            var criteria = new GetCurrencyCriteria(custId);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.True(actualIsValid);
        }
    }
}
