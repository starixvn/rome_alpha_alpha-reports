namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Criteria
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperPTCriteriaTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectSettingStatus()
        {
            // Arrange
            var criteria = new SuperPTCriteria("123456");

            // Act
            var actualSettingStatus = criteria.SettingStatus;

            // Assert
            Assert.Equal(0, actualSettingStatus);
        }

        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new SuperPTCriteria("123456");
            var expectedSettingKey = "EAS_GetPTProductBySuper";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("123456", true)]
        public void IsValid_SuperId_ReturnsExpectedValue(string superId, bool expected)
        {
            // Arrange
            var criteria = new SuperPTCriteria(superId);

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
