namespace Alpha.Reports.Tests.Models
{
    using Reports.Models;
    using Xunit;

    public class ProductTests
    {
        [Fact]
        public void Id_Always_ReturnsIdFromCtor()
        {
            // Arrange
            var productId = 1;
            var product = new Product(productId, string.Empty);

            // Act
            var actual = product.Id;

            // Assert
            Assert.Equal(productId, actual);
        }

        [Fact]
        public void Name_Always_ReturnsNameFromCtor()
        {
            // Arrange
            var productName = "product";
            var product = new Product(0, productName);

            // Act
            var actual = product.Name;

            // Assert
            Assert.Equal(productName, actual);
        }

        [Fact]
        public void DisplayName_SetFromProductName_ReturnsDisplayNameFromCtor()
        {
            // Arrange
            var productDisplayName = "productDisplayName";
            var product = new Product(0, productDisplayName, null);

            // Act
            var actual = product.DisplayName;

            // Assert
            Assert.Equal(productDisplayName, actual);
        }

        [Fact]
        public void DisplayName_SetFunctionGetDisplayName_ReturnsDisplayNameFromCtor()
        {
            // Arrange
            var productDisplayName = "productDisplayName";
            var productId = 0;
            var product = new Product(productId, string.Empty, (id) => productDisplayName);

            // Act
            var actual = product.DisplayName;

            // Assert
            Assert.Equal(productDisplayName, actual);
        }
    }
}
