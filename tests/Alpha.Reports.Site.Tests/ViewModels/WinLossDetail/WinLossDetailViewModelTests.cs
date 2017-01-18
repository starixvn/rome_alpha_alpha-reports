namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly UserFixture userFixture;

        public WinLossDetailViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        [Fact]
        public void NetTurnoverTooltips_ProductsNotContainAllBet_ReturnsEmptyList()
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);
            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());
            var expectedNetTurnoverTooltips = new List<string>();

            // Act
            var actualNetTurnoverTooltips = winLossDetailViewModel.NetTurnoverTooltips;

            // Assert
            Assert.Equal(expectedNetTurnoverTooltips, actualNetTurnoverTooltips);
        }

        [Fact]
        public void NetTurnoverTooltips_ProductsContainsAllBet_ReturnsValueContainsAllBetTooltip()
        {
            // Arrange
            var message = new WinLossDetailQuery(userFixture.User);
            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Value = AllBetProductId.ToString() }
                 });

            var expectedNetTurnoverTooltips = new List<string> { "- For AllBet, Net Turnover is total valid bets that we use to compute commission." };

            // Act
            var actualNetTurnoverTooltips = winLossDetailViewModel.NetTurnoverTooltips;

            // Assert
            Assert.Equal(expectedNetTurnoverTooltips, actualNetTurnoverTooltips);
        }

        [Fact]
        public void BuildDrillDownLink_ViewingAgentReport_CallAddQueryStringParamsFuncToBuildBetListLink()
        {
            // Arrange
            SetUpForHelpers();

            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var message = new WinLossDetailQuery(userFixture.User);
            message.CustLevelId = 2; // Agent Level
            var winLossDetailViewModel = Substitute.ForPartsOf<WinLossDetailViewModel>(
                message,
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, new List<int> { 1 }, message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Value = AllBetProductId.ToString() }
                 });
            var winlossDetailViewModel = new WinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            winLossDetailViewModel.BuildDrillDownLink(winlossDetailViewModel, "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "BetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                        (int)routeValue["Custid"] == 1
                        && (string)routeValue["Username"] == "CustName"
                        && (DateTime)routeValue["FDate"] == fromDate
                        && (DateTime)routeValue["TDate"] == toDate
                        && (string)routeValue["ProductIds"] == "1"
                        && (int)routeValue["FilterPostback"] == 1
                        && (string)routeValue["Type"] == "WLByDate"));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void BuildDrillDownLink_ViewingSuperOrMasterReport_CallReplaceOrAddQueryStringParamsFuncToBuildDrillDownLink(byte custLevelId)
        {
            // Arrange
            SetUpForHelpers();

            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var message = new WinLossDetailQuery(userFixture.User);
            message.CustLevelId = custLevelId; // Agent Level
            var winLossDetailViewModel = CreateWinLossDetailViewModel(fromDate, toDate, message);
            var winlossDetailViewModel = new WinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            winLossDetailViewModel.BuildDrillDownLink(winlossDetailViewModel, "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "currentUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                        (int)routeValue["CustId"] == 1
                        && (string)routeValue["CustName"] == "CustName"
                        && (byte)routeValue["CustLevelId"] == (custLevelId - 1)
                        && (string)routeValue["BreadcrumbLevelsJson"] == "parsedJson"));
        }

        private static void SetUpForHelpers()
        {
            Site.Helpers.JsonHelper.ToJsonFunc = Substitute.For<Func<object, string>>();
            Site.Helpers.JsonHelper.ToJsonFunc.Invoke(Arg.Any<object>()).Returns("parsedJson");
            Site.Helpers.UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapExMainPath("~/_BetList/BetList.aspx").Returns("BetListPath");
        }

        private static WinLossDetailViewModel CreateWinLossDetailViewModel(DateTime fromDate, DateTime toDate, WinLossDetailQuery message)
        {
            return Substitute.ForPartsOf<WinLossDetailViewModel>(
                message,
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, new List<int> { 1 }, message.CustLevel, "CustName", false, false),
                new ReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Value = AllBetProductId.ToString() }
                 });
        }
    }
}