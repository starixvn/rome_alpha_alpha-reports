namespace Alpha.Reports.Tests.Models.PlayForFun
{
    using Extensions;
    using Reports.Models.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    public class MasterAgentPlayForFunModelTests
    {
        [Fact]
        public void Add_Always_AddsCorrectly()
        {
            // Arrange
            var total = new MasterAgentPlayForFunModel(0, string.Empty, 1, 2, 3, 4);
            var winLoss = new MasterAgentPlayForFunModel(0, string.Empty, 5, 6, 7, 8);
            var expectedTotal = new MasterAgentPlayForFunModel(0, string.Empty, 6, 8, 10, 12);

            // Act
            total.Add(winLoss);

            // Assert
            AssertExtension.Equal(expectedTotal, total);
        }
    }
}
