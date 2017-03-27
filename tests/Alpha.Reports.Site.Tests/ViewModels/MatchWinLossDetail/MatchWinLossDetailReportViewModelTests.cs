namespace Alpha.Reports.Site.Tests.ViewModels.MatchWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Constants;
    using Extensions;
    using Fixtures;
    using Newtonsoft.Json;
    using NSubstitute;
    using Params;
    using Reports.Extensions;
    using Reports.Models;
    using Reports.Models.MatchWinLossDetail;
    using Services;
    using Site.Requests.MatchWinLossDetail;
    using Site.ViewModels.MatchWinLossDetail;
    using Xunit;

    [Trait("Category", "MatchWinLossDetail")]
    [Collection("UserCollection")]
    public class MatchWinLossDetailReportViewModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly MatchWinLossDetailQuery query;
        private readonly MatchWinLossDetailParam param;
        private readonly MatchWinLossDetailReportViewModel viewModel;

        public MatchWinLossDetailReportViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            var reportDateService = Substitute.For<ReportDateService>();
            ReportDateServiceImpl.Instance = reportDateService;
            reportDateService.GetBalanceUpDay().Returns(DefaultVariables.BalanceUpDate);

            query = new MatchWinLossDetailQuery(userFixture.User);

            anyValueCreatorFixture = new AnyValueCreatorFixture();

            param = new MatchWinLossDetailParam(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<DateTime>(),
                anyValueCreatorFixture.Create<IEnumerable<int>>(),
                anyValueCreatorFixture.Create<IEnumerable<int>>(),
                anyValueCreatorFixture.Create<UserLevel>());

            viewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: null,
                noParlayWinLossTotal: null,
                mixParlay: null,
                systemParlay: null,
                grandTotal: null,
                query: query,
                param: param,
                products: null,
                leagues: null);

            Site.Helpers.UrlHelper.MapExMainPath = (url) => url;
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = (url, routeValues) => url + "?" + JsonConvert.SerializeObject(routeValues);
        }

        [Fact]
        public void Title_Always_ReturnsCorrectTitle()
        {
            // Arrange
            var expectedTitle = "Match Win Loss Detail";

            // Act
            var atualTitle = viewModel.Title;

            // Assert
            Assert.Equal(expectedTitle, atualTitle);
        }

        [Fact]
        public void CustId_Always_ReturnsCustIdFromParam()
        {
            // Arrange
            var expectedCustId = param.CustId;

            // Act
            var actualCustId = viewModel.CustId;

            // Assert
            Assert.Equal(expectedCustId, actualCustId);
        }

        [Fact]
        public void MinDate_Always_ReturnsMinDateFromQuery()
        {
            // Arrange
            var expectedMinDate = query.MinDate;

            // Act
            var actualMinDate = viewModel.MinDate;

            // Assert
            Assert.Equal(expectedMinDate, actualMinDate);
        }

        [Fact]
        public void FromDate_Always_ReturnsFromDateFromParam()
        {
            // Arrange
            var expectedFromDate = param.FromDate;

            // Act
            var actualFromDate = viewModel.FromDate;

            // Assert
            Assert.Equal(expectedFromDate, actualFromDate);
        }

        [Fact]
        public void ToDate_Always_ReturnsToDateFromParam()
        {
            // Arrange
            var expectedToDate = param.ToDate;

            // Act
            var actualToDate = viewModel.ToDate;

            // Assert
            Assert.Equal(expectedToDate, actualToDate);
        }

        [Fact]
        public void Language_Always_ReturnsLanguageFromQuery()
        {
            // Arrange
            var expectedLanguage = query.Language;

            // Act
            var actualLanguage = viewModel.Language;

            // Assert
            Assert.Equal(expectedLanguage, actualLanguage);
        }

        [Fact]
        public void UserSelectedProductIds_Always_ReturnsUserSelectedProductIdsFromParam()
        {
            // Arrange
            var expectedUserSelectedProductIds = param.UserSelectedProductIds.JoinWithComma();

            // Act
            var actualUserSelectedProductIds = viewModel.UserSelectedProductIds;

            // Assert
            Assert.Equal(expectedUserSelectedProductIds, actualUserSelectedProductIds);
        }

        [Fact]
        public void UserSelectedLeagueIds_Always_ReturnsUserSelectedLeagueIdsFromParam()
        {
            // Arrange
            var expectedUserSelectedLeagueIds = param.UserSelectedLeagueIds.JoinWithComma();

            // Act
            var actualUserSelectedLeagueIds = viewModel.UserSelectedLeagueIds;

            // Assert
            Assert.Equal(expectedUserSelectedLeagueIds, actualUserSelectedLeagueIds);
        }

        [Fact]
        public void UserSelectedDateRangeOption_Always_ReturnsUserSelectedDateRangeOptionFromQuery()
        {
            // Arrange
            var expectedUserSelectedDateRangeOption = query.UserSelectedDateRangeOption;

            // Act
            var actualUserSelectedDateRangeOption = viewModel.UserSelectedDateRangeOption;

            // Assert
            Assert.Equal(expectedUserSelectedDateRangeOption, actualUserSelectedDateRangeOption);
        }

        [Fact]
        public void IsStackMode_Always_ReturnsIsStackModeFromQuery()
        {
            // Arrange
            var expectedIsStackMode = query.IsStackMode;

            // Act
            var actualIsStackMode = viewModel.IsStackMode;

            // Assert
            Assert.Equal(expectedIsStackMode, actualIsStackMode);
        }

        [Fact]
        public void IsFilterVisible_Always_ReturnsIsFilterVisibleFromQuery()
        {
            // Arrange
            var expectedIsFilterVisible = query.IsFilterVisible;

            // Act
            var actualIsFilterVisible = viewModel.IsFilterVisible;

            // Assert
            Assert.Equal(expectedIsFilterVisible, actualIsFilterVisible);
        }

        [Fact]
        public void HasData_HasNoParlayMatchWinLossDetail_ReturnsTrue()
        {
            // Arrange
            var reportViewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: new List<MatchWinLossDetailViewModel> { anyValueCreatorFixture.Create<MatchWinLossDetailViewModel>() },
                noParlayWinLossTotal: null,
                mixParlay: anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                systemParlay: anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                grandTotal: null,
                query: query,
                param: param,
                products: null,
                leagues: null);

            // Act
            var actualHasData = reportViewModel.HasData;

            // Assert
            Assert.True(actualHasData);
        }

        [Fact]
        public void HasData_HasMixParlay_ReturnsTrue()
        {
            // Arrange
            var reportViewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: null,
                noParlayWinLossTotal: null,
                mixParlay: anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                systemParlay: null,
                grandTotal: null,
                query: query,
                param: param,
                products: null,
                leagues: null);

            // Act
            var actualHasData = reportViewModel.HasData;

            // Assert
            Assert.True(actualHasData);
        }

        [Fact]
        public void HasData_HasSystemParlay_ReturnsTrue()
        {
            // Arrange
            var reportViewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: null,
                noParlayWinLossTotal: null,
                mixParlay: null,
                systemParlay: anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                grandTotal: null,
                query: query,
                param: param,
                products: null,
                leagues: null);

            // Act
            var actualHasData = reportViewModel.HasData;

            // Assert
            Assert.True(actualHasData);
        }

        [Fact]
        public void HasData_NoData_ReturnsFalse()
        {
            // Arrange
            var reportViewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: null,
                noParlayWinLossTotal: null,
                mixParlay: null,
                systemParlay: null,
                grandTotal: null,
                query: query,
                param: param,
                products: null,
                leagues: null);

            // Act
            var actualHasData = reportViewModel.HasData;

            // Assert
            Assert.False(actualHasData);
        }

        [Theory]
        [InlineData(4, 17)]
        [InlineData(3, 14)]
        [InlineData(2, 11)]
        public void ReportColumnsCount_SuperLevel_ReturnCorrectNumber(byte userLevelId, int expectedReportColumnsCount)
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(userLevelId);

            var reportQuery = new MatchWinLossDetailQuery(user);
            var reportViewModel = new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail: null,
                noParlayWinLossTotal: null,
                mixParlay: null,
                systemParlay: null,
                grandTotal: null,
                query: reportQuery,
                param: param,
                products: null,
                leagues: null);

            // Act
            var actualReportColumnsCount = reportViewModel.ReportColumnsCount;

            // Assert
            Assert.Equal(expectedReportColumnsCount, actualReportColumnsCount);
        }

        [Fact]
        public void IsSetDefaultStackMode_Always_ReturnsIsSetDefaultStackModeFromQuery()
        {
            // Arrange
            var expectedIsSetDefaultStackMode = query.IsSetDefaultStackMode;

            // Act
            var actualIsSetDefaultStackMode = viewModel.IsSetDefaultStackMode;

            // Assert
            Assert.Equal(expectedIsSetDefaultStackMode, actualIsSetDefaultStackMode);
        }

        [Fact]
        public void CustLevel_Always_ReturnsCustLevelFromQuery()
        {
            // Arrange
            var expectedCustLevel = query.CustLevel;

            // Act
            var actualCustLevel = viewModel.CustLevel;

            // Assert
            Assert.Equal(expectedCustLevel, actualCustLevel);
        }

        [Fact]
        public void GetBetListLink_Always_ReturnsCorrectBetListLink()
        {
            // Arrange
            var expectedBetListLink = "~/_BetList/BetList.aspx?{\"BetType\":1}";

            // Act
            var actualBetListLink = viewModel.GetBetListLink(new Dictionary<string, object>
            {
                ["BetType"] = 1,
            });

            // Assert
            Assert.Equal(expectedBetListLink, actualBetListLink);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBetListLinkRouteValues_RacingBetType_ReturnsCorrectRouteValues(bool liveIndicator)
        {
            // Arrange
            var race = 1;
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = anyValueCreatorFixture.Create<DateTime>();

            var match = new MatchWinLossDetailViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<int>(),
                race,
                anyValueCreatorFixture.Create<IEnumerable<BetTypeWinLossViewModel>>(),
                anyValueCreatorFixture.Create<BetTypeWinLossViewModel>(),
                anyValueCreatorFixture.Create<MatchWinLossDetailModel>(),
                anyValueCreatorFixture.Create<Dictionary<int, string>>());

            var betType = new BetTypeWinLossViewModel(
                anyValueCreatorFixture.Create<int>(),
                anyValueCreatorFixture.Create<string>(),
                anyValueCreatorFixture.Create<decimal>(),
                anyValueCreatorFixture.Create<decimal>(),
                liveIndicator,
                2,
                anyValueCreatorFixture.Create<NetWinLoss>(),
                anyValueCreatorFixture.Create<NetWinLoss>(),
                anyValueCreatorFixture.Create<NetWinLoss>(),
                anyValueCreatorFixture.Create<NetWinLoss>(),
                anyValueCreatorFixture.Create<NetWinLoss>());

            var expectRouteValues = new Dictionary<string, object>
            {
                ["BetType"] = betType.Id,
                ["Type"] = "WLByBettypeMatch",
                ["IsLive"] = liveIndicator ? 1 : 0,
                ["leagueid"] = match.LeagueId,
                ["fdate"] = fromDate,
                ["tdate"] = toDate,
                ["race"] = 2
            };

            // Act
            var actualRouteValues = viewModel.GetBetListLinkRouteValues(match, betType, fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectRouteValues, actualRouteValues);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBetListLinkRouteValues_NotRacingAndOutrightBetType_ReturnsCorrectRouteValues(bool liveIndicator)
        {
            // Arrange
            var race = 0;
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = anyValueCreatorFixture.Create<DateTime>();
            var match = new MatchWinLossDetailViewModel(0, string.Empty, 0, race, null, null, null, null);

            var betType = new BetTypeWinLossViewModel(BetTypeIdConstants.Outright, string.Empty, 0, 0, liveIndicator, 0, null, null, null, null, null);

            var expectRouteValues = new Dictionary<string, object>
            {
                ["BetType"] = betType.Id,
                ["Type"] = "WLByBettypeMatch",
                ["IsLive"] = liveIndicator ? 1 : 0,
                ["matchid"] = match.MatchId,
                ["leagueid"] = match.LeagueId,
                ["fdate"] = fromDate,
                ["tdate"] = toDate
            };

            // Act
            var actualRouteValues = viewModel.GetBetListLinkRouteValues(match, betType, fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectRouteValues, actualRouteValues);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetBetListLinkRouteValues_NotRacingAndNotOutrightBetType_ReturnsCorrectRouteValues(bool liveIndicator)
        {
            // Arrange
            var race = 0;
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = anyValueCreatorFixture.Create<DateTime>();
            var match = new MatchWinLossDetailViewModel(0, string.Empty, 0, race, null, null, null, null);

            var betType = new BetTypeWinLossViewModel(BetTypeIdConstants.MixParlay, string.Empty, 0, 0, liveIndicator, 0, null, null, null, null, null);

            var expectRouteValues = new Dictionary<string, object>
            {
                ["BetType"] = betType.Id,
                ["Type"] = "WLByBettypeMatch",
                ["IsLive"] = betType.IsLive ? 1 : 0,
                ["matchid"] = match.MatchId
            };

            // Act
            var actualRouteValues = viewModel.GetBetListLinkRouteValues(match, betType, fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectRouteValues, actualRouteValues);
        }

        [Fact]
        public void GetParlayBetListLinkRouteValues_Always_ReturnsCorrectRouteValues()
        {
            // Arrange
            var fromDate = anyValueCreatorFixture.Create<DateTime>();
            var toDate = anyValueCreatorFixture.Create<DateTime>();
            var betType = anyValueCreatorFixture.Create<int>();

            var expectRouteValues = new Dictionary<string, object>
            {
                ["fdate"] = fromDate,
                ["tdate"] = toDate,
                ["bettype"] = betType,
                ["type"] = "MixParlayByMatch"
            };

            // Act
            var actualRouteValues = viewModel.GetParlayBetListLinkRouteValues(betType, fromDate, toDate);

            // Assert
            AssertExtension.Equal(expectRouteValues, actualRouteValues);
        }
    }
}
