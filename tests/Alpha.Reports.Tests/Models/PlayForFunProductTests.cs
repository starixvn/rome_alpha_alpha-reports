namespace Alpha.Reports.Tests.Models
{
    using Reports.Models;
    using Xunit;

    public class PlayForFunProductTests
    {
        [Fact]
        public void Create_Always_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new PlayForFunProduct(1, "Name", true);

            // Assert
            Assert.True(criteria.IsDisable == true);
        }

        [Fact]
        public void Create_Default_SetsCorrectInitialValues()
        {
            // Act
            var criteria = new PlayForFunProduct();

            // Assert
            Assert.True(criteria.IsDisable == false);
        }
    }
}
