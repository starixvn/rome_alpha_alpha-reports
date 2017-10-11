namespace Alpha.Reports.Tests.Models
{
    using Reports.Models;
    using Xunit;

    public class ProductsByRefTableTests
    {
        [Fact]
        public void RefTable_Always_ReturnsRefTableSetFromCtor()
        {
            // Arrange
            var winLossDetailParam = new ProductsByRefTable(1, 2);

            // Act
            var actual = winLossDetailParam.RefTable;

            // Assert
            Assert.Equal(2, actual);
        }
    }
}
