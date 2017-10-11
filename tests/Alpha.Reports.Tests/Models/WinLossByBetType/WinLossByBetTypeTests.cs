namespace Alpha.Reports.Tests.Models.WinLossByBetType
{
    using Reports.Models.WinlossByBetType;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    public class WinLossByBetTypeTests
    {
        [Fact]
        public void Add_NullValue_ReturnsCurrentValue()
        {
            // Arrange
            var model = new WinLossByBetTypeModel(1, "BetTypeName", 2, 3, "SportTypeName", false, 4, 5, 6, null, null, null, null);

            // Act
            var actualValue = model.Add(null);

            // Assert
            Assert.Equal(model, actualValue);
        }
    }
}
