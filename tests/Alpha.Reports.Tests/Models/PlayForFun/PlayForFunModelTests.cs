namespace Alpha.Reports.Tests.Models.PlayForFun
{
    using Extensions;
    using Reports.Models.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    public class PlayForFunModelTests
    {
        [Fact]
        public void Add_Always_AddsCorrectly()
        {
            // Arrange
            var total = new PlayForFunModel(0, string.Empty, 1, 2, 3);
            var winLoss = new PlayForFunModel(0, string.Empty, 5, 6, 7);
            var expectedTotal = new PlayForFunModel(0, string.Empty, 6, 8, 10);

            // Act
            total.Add(winLoss);

            // Assert
            AssertExtension.Equal(expectedTotal, total);
        }
    }
}
