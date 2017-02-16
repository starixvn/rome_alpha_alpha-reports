namespace Alpha.Reports.Site.Tests.Handlers.WinLossByBetType
{
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using RemotingRef;
    using Services;
    using Site.Handlers.WinLossByBetType;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeBaseHandlerTests
    {
        private readonly WinLossByBetTypeService reportService;
        private readonly CacheRefService cacheRefService;
        private readonly WinLossByBetTypeBaseHandler winLossByBetTypeBaseHandler;

        public WinLossByBetTypeBaseHandlerTests()
        {
            reportService = Substitute.For<WinLossByBetTypeService>();
            cacheRefService = Substitute.For<CacheRefService>();

            winLossByBetTypeBaseHandler = new WinLossByBetTypeBaseHandler(reportService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            SystemTime.Today = DefaultVariables.CurrentDate;

            cacheRefService.GetProductName(23).Returns("NG2");
        }

        [Theory]     
        [InlineData(71, "Casino", false, "Casino")]
        [InlineData(1101, "Baccarat", false, "Baccarat")]
        [InlineData(1102, "Roulette", false, "Roulette")]
        [InlineData(1103, "Sicbo", false, "Sicbo")]

        [InlineData(1205, "1X2", false, "1X2 (Virtual Sports)")]
        [InlineData(1204, "Correct Score", false, "Correct Score (Virtual Sports)")]
        [InlineData(1224, "Double Change", false, "Double Change (Virtual Sports)")]
        [InlineData(1201, "Handicap", false, "Handicap (Virtual Sports)")]
        [InlineData(1203, "Over Under", false, "Over Under (Virtual Sports)")]
        [InlineData(1206, "Total Goal", false, "Total Goal (Virtual Sports)")]
        [InlineData(1231, "Win", false, "Win (Virtual Sports)")]
        [InlineData(1232, "Place", false, "Place (Virtual Sports)")]
        [InlineData(1233, "Win Place", false, "Win Place (Virtual Sports)")]
        [InlineData(1237, "Forecast", false, "Forecast (Virtual Sports)")]
        [InlineData(1238, "Tricast", false, "Tricast (Virtual Sports)")]
        [InlineData(1220, "Player Win", false, "Player Win (Virtual Sports)")]
        [InlineData(1235, "Score Bet", false, "Score Bet (Virtual Sports)")]
        [InlineData(1236, "Total Points", false, "Total Points (Virtual Sports)")]

        [InlineData(1701, "Baccarat", false, "Baccarat (Gold Deluxe)")]
        [InlineData(1702, "Roulette", false, "Roulette (Gold Deluxe)")]
        [InlineData(1703, "Parlay Baccarat", false, "Parlay Baccarat (Gold Deluxe)")]

        [InlineData(85, "Over Under", false, "Next Over Under (Number Game)")]
        [InlineData(86, "Odd Even", true, "Next Odd Even (Number Game)")]
        [InlineData(86, "Odd Even", false, "FT. Odd Even (Number Game)")]
        [InlineData(90, "Number Wheel", true, "Number Wheel (Live Game)")]
        [InlineData(90, "Number Wheel", false, "Number Wheel (Pre Game)")]
        [InlineData(4001, "", false, "Macau Games")]
        [InlineData(2502, "", false, "NG2")]
        [InlineData(2402, "Odd Even", false, "Odd Even (Allbet)")]
        public void BuildDisplayedBetTypeName_AnyBetTypeId_ReturnsCorrectBetTypeName(
            int betTypeId, 
            string betTypeName, 
            bool liveIndicator,
            string expectedDisplayedBetTypeName)
        {
            // Arrange

            // Act
            var actualDisplayedBetTypeName = winLossByBetTypeBaseHandler.BuildDisplayedBetTypeName(betTypeId, betTypeName, liveIndicator);

            // Assert
            Assert.Equal(actualDisplayedBetTypeName, expectedDisplayedBetTypeName);
        }
    }
}