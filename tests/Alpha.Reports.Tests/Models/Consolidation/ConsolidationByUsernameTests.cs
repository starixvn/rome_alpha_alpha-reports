namespace Alpha.Reports.Tests.Models.Consolidation
{
    using Alpha.Reports.Models.Consolidation;
    using Xunit;

    public class ConsolidationByUsernameTests
    {
        [Fact]
        public void Constructor_Default_ReturnsDefaultProperties()
        {
            // Arrange & Act
            var model = new ConsolidationByUsername();

            // Assert
            Assert.True(0 == model.BreakDown
                && null == model.ProductName
                && 0 == model.PositionTaking);
        }
    }
}
