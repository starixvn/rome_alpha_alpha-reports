namespace Alpha.Reports.Tests.Models.Consolidation
{
    using Alpha.Reports.Models.Consolidation;
    using Xunit;

    public class ConsolidationProductTests
    {
        [Fact]
        public void Constructor_Default_ReturnsDefaultProperties()
        {
            // Arrange & Act
            var model = new ConsolidationProduct();

            // Assert
            Assert.True(0 == model.Id
                && null == model.Name
                && false == model.IsEnable
                && null == model.EndOfSupportDate
                && 0 == model.Type
                && 0 == model.OrderNo);
        }
    }
}
