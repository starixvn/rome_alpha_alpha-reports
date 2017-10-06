namespace Alpha.Reports.Tests.Criteria
{
    using Reports.Criteria;
    using Xunit;

    [Trait("Category", "BaseFunction")]
    public class GetProductsByRefTableTests
    {
        [Fact]
        public void GetSettingKey_Always_ReturnsValidSettingKey()
        {
            // Arrange
            var criteria = new GetProductsByRefTableCriteria(string.Empty);
            var expectedSettingKey = "Age_Rpt_GetProductListByRefTable";

            // Act
            var actualSettingKey = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expectedSettingKey, actualSettingKey);
        }

        [Fact]
        public void IsValid_Always_ReturnsValid()
        {
            // Arrange
            var criteria = new GetProductsByRefTableCriteria(string.Empty);
            var expectedIsValid = true;

            // Act
            var actualIsValid = criteria.IsValid();

            // Assert
            Assert.Equal(expectedIsValid, actualIsValid);
        }

        [Fact]
        public void RefTable_Always_ReturnsRefTableSetFromCtor()
        {
            // Arrange
            var winLossDetailParam = new GetProductsByRefTableCriteria("2");

            // Act
            var actual = winLossDetailParam.RefTable;

            // Assert
            Assert.Equal("2", actual);
        }
    }
}
