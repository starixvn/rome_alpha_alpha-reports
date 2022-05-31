namespace Alpha.Reports.Site.Tests.Handlers.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Constants;
    using Extensions;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Constants;
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
            reportDateService.GetBalanceUpDay().Returns(DefaultVariablesTest.BalanceUpDate);

            matchWinLossDetailHandler = new MatchWinLossDetailHandler(productService, leagueService, reportService, cacheRefService);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Constants.LanguageCode.English);
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
        public void HandleWinLossDetailQuery_DefaultLoading_NotCallGetLeaguesByCustomer()
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User)
            {
                UserSelectedLeagueIds = string.Empty,
                IsUserSelectsAllLeagues = false
            };

            // Act
            matchWinLossDetailHandler.Handle(query);

            // Assert
            leagueService.GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>()).DidNotReceive();
        }

        [Fact]
        public void HandleWinLossDetailQuery_UserSelectAllLeagues_NotCallGetLeaguesByCustomer()
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User)
            {
                UserSelectedLeagueIds = string.Empty,
                IsUserSelectsAllLeagues = true
            };

            // Act
            matchWinLossDetailHandler.Handle(query);

            // Assert
            leagueService.Received().GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HandleWinLossDetailQuery_UserSelectSomeLeagues_NotCallGetLeaguesByCustomer(bool isUserSelectsAllLeagues)
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User)
            {
                UserSelectedLeagueIds = "1,2",
                IsUserSelectsAllLeagues = isUserSelectsAllLeagues
            };

            // Act
            matchWinLossDetailHandler.Handle(query);

            // Assert
            leagueService.Received().GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>());
        }

        [Fact]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void HandleWinLossDetailQuery_DefaultLoading_ReturnsCorrectViewModel()
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User)
            {
                UserSelectedLeagueIds = string.Empty
            };

            reportService.GetMatchWinLossDetail(Arg.Any<MatchWinLossDetailParam>()).Returns(new List<MatchWinLossDetailModel>
            {
                CreateMatchWinLossDetailModel(betType: 1, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 2, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 9, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true),
                CreateMatchWinLossDetailModel(betType: 29, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true),
                CreateMatchWinLossDetailModel(betType: 5301, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true)
            });

            leagueService.GetLeagueName(Arg.Any<int>(),Arg.Any<byte>()).Returns("League 1");

            reportService.GetProductsForCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(1, "Product 1", null, 0, 0),
                new Product(2, "Product 2", null, 0, 0)
            });
            query.UserSelectedProductIds = "1";
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new List<int> { 1 });

            var expectedViewModel = new MatchWinLossDetailReportViewModel(
                new List<MatchWinLossDetailViewModel>
                {
                    new MatchWinLossDetailViewModel(
                        1,
                        "League 1",
                        1,
                        1,
                        new List<BetTypeWinLossViewModel>
                        {
                            CreateBetTypeWinLossViewModel(1, "Bet Type 1", 1, 1, 1),
                            CreateBetTypeWinLossViewModel(2, "Bet Type 2", 1, 1, 1)
                        },
                        CreateBetTypeWinLossViewModel(0, null, 2, 2, 0),
                        new MatchWinLossDetailModel(false, 1, 1, 1, 1, false, 1,(byte)LeagueType.Self, 1, string.Empty, false, string.Empty, 0, 0, null, null, null, null),
                        "Team 1",
                        "Team 1")
                },
                CreateBetTypeWinLossViewModel(0, null, 2, 2, 0),
                CreateBetTypeWinLossViewModel(9, "Bet Type 9", 1, 1, 1),
                CreateBetTypeWinLossViewModel(29, "Bet Type 29", 1, 1, 1),
                new List<BetTypeWinLossViewModel> { CreateBetTypeWinLossViewModel(5301, "Bet Type 5301", 1, 1, 1) },
                CreateBetTypeWinLossViewModel(0, null, 5, 5, 0),
                query,
                new MatchWinLossDetailParam(
                     query.CustId,
                     query.Site,
                     query.CurrencyId,
                     query.FromDate,
                     query.ToDate,
                     query.LimitedDateToShowInfo,
                     Enumerable.Empty<string>(),
                     new List<int> { 1 },
                     query.CustLevel),
                new List<GroupProductViewModel>
                {
                    new GroupProductViewModel(
                        new SelectListGroup(),
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
                        })
                },
                Enumerable.Empty<SelectListItem>());

            // Act
            var actualViewModel = matchWinLossDetailHandler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [Theory]
        [InlineData(null, true, true, true)]
        [InlineData("", true, true, true)]
        [InlineData(" ", true, true, true)]
        [InlineData("  ", true, true, true)]
        [InlineData("2_0", false, false, true)]
        public void HandleWinLossDetailQuery_NotNullMessage_ReturnsCorrectViewModel(
            string userSelectedLeagueIds,
            bool isUserSelectsAllLeagues,
            bool isSelectedFirstLeage,
            bool isSelectedLeage)
        {
            // Arrange
            var query = new MatchWinLossDetailQuery(userFixture.User);

            reportService.GetMatchWinLossDetail(Arg.Any<MatchWinLossDetailParam>()).Returns(new List<MatchWinLossDetailModel>
            {
                CreateMatchWinLossDetailModel(betType: 1, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 2, showTime: "Show Time 1"),
                CreateMatchWinLossDetailModel(betType: 9, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true),
                CreateMatchWinLossDetailModel(betType: 29, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true),
                CreateMatchWinLossDetailModel(betType: 5301, showTime: "Show Time 1", liveIndicator: false, isNeutral: false, isParlay: true)
            });

            leagueService.GetLeagueName(Arg.Any<int>(), Arg.Any<byte>()).Returns("League 1");

            reportService.GetProductsForCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>()).Returns(new List<Product>
            {
                new Product(1, "Product 1", null, 0, 0),
                new Product(2, "Product 2", null, 0, 0)
            });
            query.UserSelectedProductIds = "1";
            productService.FilterProductIdsByCustomer(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new List<int> { 1 });

            leagueService.GetLeaguesByCustomer(Arg.Any<GetLeaguesByCustomerParam>()).Returns(new List<League>
            {
                new League(1, 1, "League 1", 0),
                new League(2, 2, "League 2", 0)
            });

            query.UserSelectedLeagueIds = userSelectedLeagueIds;
            query.IsUserSelectsAllLeagues = isUserSelectsAllLeagues;
            var leagueIds = (userSelectedLeagueIds ?? string.Empty).ToListString();

            var expectedViewModel = new MatchWinLossDetailReportViewModel(
                new List<MatchWinLossDetailViewModel>
                {
                    new MatchWinLossDetailViewModel(
                        1,
                        "League 1",
                        1,
                        1,
                        new List<BetTypeWinLossViewModel>
                        {
                            CreateBetTypeWinLossViewModel(1, "Bet Type 1", 1, 1, 1),
                            CreateBetTypeWinLossViewModel(2, "Bet Type 2", 1, 1, 1)
                        },
                        CreateBetTypeWinLossViewModel(0, null, 2, 2, 0),
                        new MatchWinLossDetailModel(false, 1, 1, 1, 1, false, 1,(byte)LeagueType.Self, 1, string.Empty, false, string.Empty, 0, 0, null, null, null, null),
                        "Team 1",
                        "Team 1")
                },
                CreateBetTypeWinLossViewModel(0, null, 2, 2, 0),
                CreateBetTypeWinLossViewModel(9, "Bet Type 9", 1, 1, 1),
                CreateBetTypeWinLossViewModel(29, "Bet Type 29", 1, 1, 1),
                new List<BetTypeWinLossViewModel> { CreateBetTypeWinLossViewModel(5301, "Bet Type 5301", 1, 1, 1) },
                CreateBetTypeWinLossViewModel(0, null, 5, 5, 0),
                query,
                new MatchWinLossDetailParam(
                     query.CustId,
                     query.Site,
                     query.CurrencyId,
                     query.FromDate,
                     query.ToDate,
                     query.LimitedDateToShowInfo,
                     leagueIds,
                     new List<int> { 1 },
                     query.CustLevel),
                new List<GroupProductViewModel>
                {
                    new GroupProductViewModel(
                        new SelectListGroup(),
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
                        })
                },
                new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "League 1",
                        Value = "1_0",
                        Selected = isSelectedFirstLeage
                    },
                    new SelectListItem
                    {
                        Text = "League 2",
                        Value = "2_0",
                        Selected = isSelectedLeage
                    }
                });

            // Act
            var actualViewModel = matchWinLossDetailHandler.Handle(query);

            // Assert
            AssertExtension.Equal(expectedViewModel, actualViewModel);
        }       

        [Fact]
        public void GetTeamName_ValidTeamId_ReturnsCorrectTeamNames()
        {
            // Arrange
            var teamId = 1;
            var expectedTeamNames = "Team 1";

            // Act
            var actualTeamName = matchWinLossDetailHandler.GetTeamName(teamId, (byte)LeagueType.Self);

            // Assert
            Assert.Equal(expectedTeamNames, actualTeamName);
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
        public void BuildWinLossTotal_EmptyData_ReturnsNull()
        {
            // Arrange
            var data = Enumerable.Empty<MatchWinLossDetailViewModel>();

            // Act
            var actualTotal = matchWinLossDetailHandler.BuildWinLossTotal(data);

            // Assert
            Assert.Null(actualTotal);
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
                leagueType: (byte)LeagueType.Self,
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

        private BetTypeWinLossViewModel CreateBetTypeWinLossViewModel(int id, string name, decimal turnover, decimal grossComm, int race = 0)
            => new BetTypeWinLossViewModel(
                id,
                name,
                turnover,
                grossComm,
                false,
                race,
                null,
                null,
                null,
                null,
                new NetWinLoss(0, 0));
    }
}
