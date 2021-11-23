namespace Alpha.Reports.Tests.Criteria.Consolidation
{
    using Alpha.Reports.Criteria.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class GetSMAPositionTakingCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetSMAPositionTakingCriteria("1,2,3,4,5,6");
            var expectedSettingKey = "EAS_GetPTProductBySMA";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("     ", false)]
        [InlineData("1,2,3,", true)]
        public void IsValid_CustIds_ReturnsCorrectExpected(string custIds, bool expected)
        {
            // Arrange
            var criteria = new GetSMAPositionTakingCriteria(custIds);

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actualIsValid);
        }

        [Fact]
        public void SettingStatus_Always_ReturnsZero()
        {
            // Arrange & Act
            var criteria = new GetSMAPositionTakingCriteria("1,2,3,4,5,6");

            // Assert
            Assert.Equal(0, criteria.SettingStatus);
        }
    }
}
