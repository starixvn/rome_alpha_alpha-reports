namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByBetType
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using RemotingRef;
    using Reports.Models.WinlossByBetType;
    using Services;
    using Site.Requests.WinLossByBetType;
    using Site.ViewModels.WinLossByBetType;
    using TestCases;
    using Utils;
    using Xunit;

    [Trait("Category", "WinLossByBetType")]
    [Collection("UserCollection")]
    public class WinLossByBetTypeViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly CacheRefService cacheRefService;
        private readonly ReportDateService reportDateService;
        private readonly WinLossByBetTypeViewModel winlossByBettypeViewModel;
        private readonly CompareLogic compareLogic;

        public WinLossByBetTypeViewModelTests(UserFixture userFixture)
        {
            cacheRefService = Substitute.For<CacheRefService>();
            reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(SystemTime.Today);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.userFixture = userFixture;
            SystemTime.Today = DefaultVariables.CurrentDate;
            compareLogic = new CompareLogic();

            var message = CreateWinlossByBetTypeQuery(4);
             winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
                Enumerable.Empty<WinLossByBetTypeModel>(),
                fixture.Create<WinLossByBetTypeModel>(),
                message,
                fixture.Create<WinLossByBetTypeParam>(),
                fixture.Create<Func<int, string, bool, string>>(),
                cacheRefService);

            cacheRefService.GetProductName(23).Returns("Macau Game");
            cacheRefService.GetProductName(24).Returns("NG2");
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var expectedTitle = "Win Loss By Bet Type";
            
            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.Title, expectedTitle);
        }

        [Theory]
        [MemberData("WinLossGameTypeInfoViewModelSources", MemberType = typeof(WinLossGameTypeInfoViewModelCases))]
        public void BuildWinLossGameTypeInfoViewModel_Always_ReturnsCorrectData(
          int betTypeId,
          int productId,
          WinLossGameTypeInfoViewModel expectedWinLossGameTypeInfoViewModel)
        {
            // Arrange

            // Act
            var actualWinLossGameTypeInfoViewModel = winlossByBettypeViewModel.BuildWinLossGameTypeInfoViewModel(betTypeId, productId);

            // Assert
            var result = compareLogic.Compare(expectedWinLossGameTypeInfoViewModel, actualWinLossGameTypeInfoViewModel);
            Assert.True(result.AreEqual);
        }

        private WinLossByBetTypeQuery CreateWinlossByBetTypeQuery(byte custLevelId)
        {
            var winLossBettypeQuery = new WinLossByBetTypeQuery(userFixture.User);
            winLossBettypeQuery.CustLevelId = custLevelId;
            winLossBettypeQuery.FromDate = DefaultVariables.CurrentDate;
            winLossBettypeQuery.ToDate = DefaultVariables.CurrentDate;

            return winLossBettypeQuery;
        }
    }
}