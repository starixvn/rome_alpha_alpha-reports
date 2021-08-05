namespace Alpha.Reports.Tests.Features.SuperWinLossDetailByPT.Params
{
    using Alpha.Reports.Features.SuperWinLossDetailByPT.Params;
    using Xunit;

    [Trait("Category", "SuperWinLossDetailByPT")]
    public class SuperPTParamTests
    {
        [Fact]
        public void Constructor_Always_AssignsCorrectSuperId()
        {
            // Arrange & Act
            var param = new SuperPTParam(123456);

            // Assert
            Assert.Equal("123456", param.SuperId);
        }
    }
}
