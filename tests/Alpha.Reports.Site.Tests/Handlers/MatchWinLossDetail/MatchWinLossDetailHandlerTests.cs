namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using RemotingRef;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Services;
    using Site.Handlers.MatchWinLossDetail;
    using Site.Requests.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("UserCollection")]
    public class MatchWinLossDetailHandlerTests
    {
        private readonly UserFixture userFixture;
        private readonly ProductService productService;
        private readonly LeagueService leagueService;
        private readonly MatchWinLossDetailService reportService;
        private readonly MatchWinLossDetailHandler matchWinLossDetailHandler;

        public MatchWinLossDetailHandlerTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;
            productService = Substitute.For<ProductService>();
            leagueService = Substitute.For<LeagueService>();

            var cacheRefService = Substitute.For<CacheRefService>();
            cacheRefService.GetTeamName(Arg.Any<int>()).Returns(callInfo => "Team " + callInfo.Args()[0]);
            cacheRefService.GetBetType(Arg.Any<int>()).Returns(callInfo => "Bet Type " + callInfo.Args()[0]);

            reportService = Substitute.For<MatchWinLossDetailService>();

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);

            matchWinLossDetailHandler = new MatchWinLossDetailHandler(productService, leagueService, reportService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void HandleWinLossDetailQuery_NullMessage_ThrowsArgumentNullException()
        {
            // Arrange
            MatchWinLossDetailQuery nullMessage = null;

            // Act &  Assert
            Assert.Throws<ArgumentNullException>(() => matchWinLossDetailHandler.Handle(nullMessage));
        }

        [Fact]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void HandleWinLossDetailQuery_NotNullMessage_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User);

            reportService.GetMatchWinLossDetail(Arg.Any<MatchWinLossDetailParam>()).Returns(new List<MatchWinLossDetailModel>
            {
                CreateMatchWinLossDetailModel(betType: 1, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 2, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 9, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true),
                CreateMatchWinLossDetailModel(betType: 29, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true)
            });

            leagueService.GetLeagueNames(Arg.Any<IEnumerable<int>>()).Returns(new Dictionary<int, string>
            {
                [1] = "League 1"
            });

            reportService.GetProductsForCustomer(Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(1, "Product 1"),
                new Product(2, "Product 2")
            });
            query.UserSelectedProductIds = "1";
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new List<int> { 1 });

            leagueService.GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>()).Returns(new List<League>
            {
                new League(1, 1, "League 1"),
                new League(2, 2, "League 2")
            });

            var leagueIds = new List<int> { 2 };
            query.UserSelectedLeagueIds = leagueIds.JoinWithComma();

            var expectedViewModel = new MatchWinLossDetailReportViewModel(
                new List<MatchWinLossDetailViewModel>
                {
                    new MatchWinLossDetailViewModel(
                        1,
                        "League 1",
                        1,
                        "Team 1-vs-Team 1",
                        1,
                        new List<BetTypeWinLossViewModel>
                        {
                            CreateBetTypeWinLossViewModel(1, "Bet Type 1", 1, 1),
                            CreateBetTypeWinLossViewModel(2, "Bet Type 2", 1, 1)
                        },
                        CreateBetTypeWinLossViewModel(0, null, 2, 2))
                },
                CreateBetTypeWinLossViewModel(0, null, 2, 2),
                CreateBetTypeWinLossViewModel(9, "Bet Type 9", 1, 1),
                CreateBetTypeWinLossViewModel(29, "Bet Type 29", 1, 1),
                CreateBetTypeWinLossViewModel(0, null, 4, 4),
                query,
                new MatchWinLossDetailParam(
                     query.CustId,
                     query.FromDate,
                     query.ToDate,
                     leagueIds,
                     new List<int> { 1 },
                     query.CustLevel),
                new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = null,
                        Value = "1",
                        Selected = true
                    },
                    new SelectListItem
                    {
                        Text = null,
                        Value = "2",
                        Selected = false
                    }
                },
                new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "League 1",
                        Value = "1",
                        Selected = false
                    },
                    new SelectListItem
                    {
                        Text = "League 2",
                        Value = "2",
                        Selected = true
                    }
                });

            // Act
            var actualViewModel = matchWinLossDetailHandler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [Fact]
        public void GetTeamNames_NullTeamIds_ReturnsEmpty()
        {
            // Arrange
            IEnumerable<int> teamIds = null;

            // Act
            var actualTeamNames = matchWinLossDetailHandler.GetTeamNames(teamIds);

            // Assert
            Assert.Empty(actualTeamNames);
        }

        [Fact]
        public void GetTeamNames_NotNullTeamIds_ReturnsCorrectTeamNames()
        {
            // Arrange
            var teamIds = new List<int> { 1 };
            var expectedTeamNames = new Dictionary<int, string>
            {
                [1] = "Team 1"
            };

            // Act
            var actualTeamNames = matchWinLossDetailHandler.GetTeamNames(teamIds);

            // Assert
            Assert.Equal(expectedTeamNames, actualTeamNames);
        }

        [Fact]
        public void GetBetTypeNames_NullBetTypeIds_ReturnsEmpty()
        {
            // Arrange
            IEnumerable<int> betTypeIds = null;

            // Act
            var actualBetTypeNames = matchWinLossDetailHandler.GetBetTypeNames(betTypeIds);

            // Assert
            Assert.Empty(actualBetTypeNames);
        }

        [Fact]
        public void GetBetTypeNames_NotNullBetTypeIds_ReturnsCorrectBetTypeNames()
        {
            // Arrange
            var betTypeIds = new List<int> { 1 };
            var expectedBetTypeNames = new Dictionary<int, string>
            {
                [1] = "Bet Type 1"
            };

            // Act
            var actualBetTypeNames = matchWinLossDetailHandler.GetBetTypeNames(betTypeIds);

            // Assert
            Assert.Equal(expectedBetTypeNames, actualBetTypeNames);
        }

        [Theory]
        [InlineData(10, "League Name_Match1")]
        [InlineData(31, "League Name_Race1")]
        [InlineData(81, "League Name_MatchCode1")]
        [InlineData(1239, "League Name_MatchId1")]
        public void GetGroupId_NotVirtualHorseRacingBetType_ReturnsCorrectId(int betType, string expectedId)
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: betType, showTime: "Show Time 1");
            var leagueName = "League Name";

            // Act
            var actualId = matchWinLossDetailHandler.GetGroupId(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void GetGroupId_VirtualHorseRacingBetTypeAndValidShowTime_ReturnsCorrectId()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1231, showTime: "01/01/2017");
            var leagueName = "League Name";
            var expectedId = "League Name_ShowTime01/01/2017";

            // Act
            var actualId = matchWinLossDetailHandler.GetGroupId(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void GetGroupId_VirtualHorseRacingBetTypeAndInvalidShowTime_ReturnsCorrectId()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1231, showTime: "Show Time 1");
            var leagueName = "League Name";
            var expectedId = "League Name";

            // Act
            var actualId = matchWinLossDetailHandler.GetGroupId(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public void GetGroupId_OutrightBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 10, showTime: "Show Time 1");
            var expectedTitle = string.Empty;

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, new Dictionary<int, string>());

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_RacingBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 31, showTime: "Show Time 1");
            var expectedTitle = "Race 1";

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, new Dictionary<int, string>());

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_NumberGameBetType_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 91, showTime: "Show Time 1");
            var expectedTitle = "Number Game No 1";

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, new Dictionary<int, string>());

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_HorseRacingAndValidShowTime_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1231, showTime: "01/01/2017");
            var expectedTitle = "01/01/2017 12:00:00 AM";

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, new Dictionary<int, string>());

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_HorseRacingAndInvalidShowTime_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1231, showTime: "Show Time 1");
            var expectedTitle = string.Empty;

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, new Dictionary<int, string>());

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_OtherBetTypeAndNeutral_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1239, showTime: "Show Time 1", liveIndicator: false, isNeutral: true);
            var expectedTitle = "Team 1 (N)-vs-Team 1";
            var teamName = new Dictionary<int, string>
            {
                [1] = "Team 1"
            };

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, teamName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetGroupId_OtherBetTypeAndNotNeutral_ReturnsCorrectTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1239, showTime: "Show Time 1");
            var expectedTitle = "Team 1-vs-Team 1";
            var teamName = new Dictionary<int, string>
            {
                [1] = "Team 1"
            };

            // Act
            var actualTitle = matchWinLossDetailHandler.GetMatchTitle(matchWinLossDetailModel, teamName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetLeagueTitle_RacingBetType_ReturnsCorrectLeagueTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 31, showTime: "Show Time 1");
            var leagueName = "League Name";
            var expectedTitle = "Racing - League Name";

            // Act
            var actualTitle = matchWinLossDetailHandler.GetLeagueTitle(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData("", "Number Game")]
        [InlineData("League Name", "Number Game - League Name")]
        public void GetLeagueTitle_NumberGameBetType_ReturnsCorrectLeagueTitle(string leagueName, string expectedTitle)
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 91, showTime: "Show Time 1");

            // Act
            var actualTitle = matchWinLossDetailHandler.GetLeagueTitle(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData("01/01/2017", "League Name")]
        [InlineData("Invalid Show Time", "League Name")]
        public void GetLeagueTitle_VirtualHorseRacingBetType_ReturnsCorrectLeagueTitle(string showTime, string expectedTitle)
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1231, showTime: showTime);
            var leagueName = "League Name";

            // Act
            var actualTitle = matchWinLossDetailHandler.GetLeagueTitle(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Fact]
        public void GetLeagueTitle_OtherBetTypes_ReturnsCorrectLeagueTitle()
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: 1239, showTime: "Show Time 1");
            var leagueName = "League Name";
            var expectedTitle = leagueName;

            // Act
            var actualTitle = matchWinLossDetailHandler.GetLeagueTitle(matchWinLossDetailModel, leagueName);

            // Assert
            Assert.Equal(expectedTitle, actualTitle);
        }

        [Theory]
        [InlineData(85, true, "Next ")]
        [InlineData(85, false, "Next ")]
        [InlineData(86, true, "Next ")]
        [InlineData(86, false, "FT ")]
        [InlineData(100, true, "")]
        [InlineData(100, false, "")]
        public void GetBetTypePrefix_Always_ReturnsCorrectPrefix(
            int betType,
            bool liveIndicator,
            string expectedPrefix)
        {
            // Arrange
            var matchWinLossDetailModel = CreateMatchWinLossDetailModel(betType: betType, showTime: "Show Time 1", liveIndicator: liveIndicator);

            // Act
            var actualPrefix = matchWinLossDetailHandler.GetBetTypePrefix(matchWinLossDetailModel);

            // Assert
            Assert.Equal(expectedPrefix, actualPrefix);
        }

        private MatchWinLossDetailModel CreateMatchWinLossDetailModel(
            int betType,
            string showTime,
            bool liveIndicator = false,
            bool isNeutral = false,
            bool isParlay = false)
            => new MatchWinLossDetailModel(
                isParlay: isParlay,
                matchId: 1,
                homeId: 1,
                awayId: 1,
                betType: betType,
                liveIndicator: liveIndicator,
                leagueId: 1,
                race: 1,
                matchCode: "1",
                isNeutral: isNeutral,
                showTime: showTime,
                turnover: 1,
                grossComm: 1,
                memberNetWinLoss: null,
                agentNetWinLoss: null,
                masterNetWinLoss: null,
                superNetWinLoss: null);

        private BetTypeWinLossViewModel CreateBetTypeWinLossViewModel(int id, string name, decimal turnover, decimal grossComm)
            => new BetTypeWinLossViewModel(
                id,
                name,
                turnover,
                grossComm,
                false,
                null,
                null,
                null,
                null,
                new NetWinLoss(0, 0));
    }
}
