namespace Alpha.Reports.Site.Tests.ViewModels.WinLossByBetType
{
    using System;
    using System.Collections.Generic;
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
    using Reports.Models;
    using Reports.Models.WinlossByBetType;
    using Services;
    using Site.Requests.WinLossByBetType;
    using Site.ViewModels.WinLossByBetType;
    using Sunplus.Agent.Model.BL.Definitions;
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

            cacheRefService.GetProductName(23).Returns("Macau Game");
            cacheRefService.GetProductName(24).Returns("NG2");
        }

        [Fact]
        public void Title_WinLossLevel_ReturnCorrectTitle()
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(4);
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
               Enumerable.Empty<WinLossByBetTypeModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByBetTypeParam>(),
               fixture.Create<Func<BetType, string, bool, string>>(),
               cacheRefService);
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
            var message = CreateWinlossByBetTypeQuery(4);
            
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
               Enumerable.Empty<WinLossByBetTypeModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByBetTypeParam>(),
               fixture.Create<Func<BetType, string, bool, string>>(),
               cacheRefService);

            // Act
            var actualWinLossGameTypeInfoViewModel = winlossByBettypeViewModel.BuildWinLossGameTypeInfoViewModel((BetType)betTypeId, productId);

            // Assert
            var result = compareLogic.Compare(expectedWinLossGameTypeInfoViewModel, actualWinLossGameTypeInfoViewModel);
            Assert.True(result.AreEqual);
        }

        [Theory]
        [InlineData(4, "Super")]
        [InlineData(3, "Master")]
        [InlineData(2, "Agent")]
        public void BuildResources_CustLevel_ReturnCorrectCustLevelName(
        byte custLevelId,
        string expectedCustLevelName)
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
               Enumerable.Empty<WinLossByBetTypeModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByBetTypeParam>(),
               fixture.Create<Func<BetType, string, bool, string>>(),
               cacheRefService);

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.CustLevelName, expectedCustLevelName);
        }

        [Theory]
        [InlineData(4, "Master")]
        [InlineData(3, "Agent")]
        [InlineData(2, "Member")]
        public void BuildResources_CustLevel_ReturnCorrectDownLineLevelName(
        byte custLevelId,
        string expectedDownLineLevelName)
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(custLevelId);
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
               Enumerable.Empty<WinLossByBetTypeModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               fixture.Create<WinLossByBetTypeParam>(),
               fixture.Create<Func<BetType, string, bool, string>>(),
               cacheRefService);

            // Act & Assert
            Assert.Equal(winlossByBettypeViewModel.DownLineLevelName, expectedDownLineLevelName);
        }

        [Fact]
        public void BuildWinLossByGameTypePath_ViewWinLossByGameType_CallAddQueryStringParamsFuncToBuilWinLossByGameTypePath()
        {
            // Arrange
            var message = CreateWinlossByBetTypeQuery(4);
            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var winlossByBettypeParam = new WinLossByBetTypeParam(1, fromDate, toDate, UserLevel.Agent);
            var winlossByBettypeViewModel = new WinLossByBetTypeViewModel(
               Enumerable.Empty<WinLossByBetTypeModel>(),
               fixture.Create<WinLossByBetTypeModel>(),
               message,
               winlossByBettypeParam,
               fixture.Create<Func<BetType, string, bool, string>>(),
               cacheRefService);
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            winlossByBettypeViewModel.BuildWinLossByGameTypePath(22, "Casino Win Loss", "WinLossByGameTypePath");

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "WinLossByGameTypePath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                        (int)routeValue["UserSelectedDateRangeOption"] == 0
                        && (DateTime)routeValue["FromDate"] == fromDate
                        && (DateTime)routeValue["ToDate"] == toDate
                        && (int)routeValue["ProductId"] == 22
                        && (bool)routeValue["IsStackMode"] == false
                        && (bool)routeValue["IsDescendingSort"] == false
                        && (bool)routeValue["IsFilterVisible"] == true
                        && (string)routeValue["Title"] == "Casino Win Loss"
                        && (bool)routeValue["IsSetDefaultStackMode"] == false));
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