namespace Alpha.Reports.Site.Tests.Handlers.WinLossByProduct
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using NSubstitute;
    using RemotingRef;
    using Services;
    using Site.Handlers.WinLossByProduct;
    using Sunplus.Agent.Model.BL.Definitions;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByProduct")]
    [Collection("UserCollection")]
    public class WinLossByProductBaseHandlerTests
    {
        private readonly ProductService productService;
        private readonly WinLossByProductService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossByProductBaseHandler winLossByBetTypeBaseHandler;

        public WinLossByProductBaseHandlerTests()
        {
            reportService = Substitute.For<WinLossByProductService>();
            cacheRefService = Substitute.For<CacheRefService>();
            productService = Substitute.For<ProductService>();

            winLossByBetTypeBaseHandler = new WinLossByProductBaseHandler(reportService, productService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariablesTest.CurrentDate;

            cacheRefService.GetProductName(23).Returns("NG2");
        }

        [Theory]
        [InlineData(6, 71, "Casino", false, "Casino")]
        [InlineData(4, 1101, "Baccarat", false, "Baccarat")]
        [InlineData(4, 1102, "Roulette", false, "Roulette")]
        [InlineData(4, 1103, "Sicbo", false, "Sicbo")]

        [InlineData(5, 1205, "1X2", false, "1X2")]
        [InlineData(5, 1204, "Correct Score", false, "Correct Score")]
        [InlineData(5, 1224, "Double Change", false, "Double Change")]
        [InlineData(5, 1201, "Handicap", false, "Handicap")]
        [InlineData(5, 1203, "Over Under", false, "Over Under")]
        [InlineData(5, 1206, "Total Goal", false, "Total Goal")]
        [InlineData(5, 1231, "Win", false, "Win")]
        [InlineData(5, 1232, "Place", false, "Place")]
        [InlineData(5, 1233, "Win Place", false, "Win Place")]
        [InlineData(5, 1237, "Forecast", false, "Forecast")]
        [InlineData(5, 1238, "Tricast", false, "Tricast")]
        [InlineData(5, 1220, "Player Win", false, "Player Win")]
        [InlineData(5, 1235, "Score Bet", false, "Score Bet")]
        [InlineData(5, 1236, "Total Points", false, "Total Points")]

        [InlineData(12, 1701, "Baccarat", false, "Baccarat")]
        [InlineData(12, 1702, "Roulette", false, "Roulette")]
        [InlineData(12, 1703, "Parlay Baccarat", false, "Parlay Baccarat")]

        [InlineData(3, 85, "Over Under", false, "Next Over Under")]
        [InlineData(3, 86, "Odd Even", true, "Next Odd Even")]
        [InlineData(3, 86, "Odd Even", false, "FT. Odd Even")]
        [InlineData(3, 90, "Number Wheel", true, "Number Wheel (Live Game)")]
        [InlineData(3, 90, "Number Wheel", false, "Number Wheel (Pre Game)")]
        [InlineData(22, 4001, "Macau Games", false, "Macau Games")]
        [InlineData(23, 2502, "NG2", false, "NG2")]
        [InlineData(21, 2402, "Odd Even", false, "Odd Even")]
        public void BuildDisplayedBetTypeName_AnyBetTypeId_ReturnsCorrectBetTypeName(
            int productId,
            int betTypeId,
            string betTypeName,
            bool liveIndicator,
            string expectedDisplayedBetTypeName)
        {
            // Arrange

            // Act
            var actualDisplayedBetTypeName = winLossByBetTypeBaseHandler.BuildDisplayedBetTypeName(productId, (BetType)betTypeId, betTypeName, liveIndicator);

            // Assert
            Assert.Equal(actualDisplayedBetTypeName, expectedDisplayedBetTypeName);
        }
    }
}