namespace Alpha.Reports.Tests.Models.WinLossAnalysis
{
    using Reports.Models.WinLossAnalysis;
    using Xunit;

    [Trait("Category", "WinLossAnalysis")]
    public class WinLossAnalysisModelTests
    {
        [Fact]
        public void Add_NullValue_ReturnsCurrentValue()
        {
            // Arrange
            var model = new WinLossAnalysisModel("RollUpKey", 1, 2, 3, null, null, null, null);

            // Act
            var actualValue = model.Add(null);

            // Assert
            Assert.Equal(model, actualValue);
        }
    }
}
