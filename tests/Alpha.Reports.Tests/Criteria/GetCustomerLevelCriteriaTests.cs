namespace Alpha.Reports.Tests.Criteria
{
    using Alpha.Reports.Criteria;
    using Xunit;

    public class GetCustomerLevelCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetCustomerLevelCriteria("master", 123456);
            var expectedSettingKey = "Age_Rpt_GetCustomerInfo";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(-123456, false)]
        [InlineData(0, false)]
        [InlineData(123456, true)]
        public void IsValid_CustId_ReturnsExpectedResults(int custId, bool expected)
        {
            // Arrange
            var criteria = new GetCustomerLevelCriteria("master", custId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("      ", false)]
        [InlineData("agent", true)]
        public void IsValid_Username_ReturnsExpectedResults(string username, bool expected)
        {
            // Arrange
            var criteria = new GetCustomerLevelCriteria(username, 123456);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
