namespace Alpha.Reports.Site.Tests.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Alpha.Reports.Site.ViewModels.Components;
    using Constants;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Ploeh.AutoFixture;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Site.Requests.WinLoss;
    using Site.ViewModels.WinLoss;
    using Xunit;

    [Trait("Category", "WinLoss")]
    [Collection("UserCollection")]
    public class WinLossViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly Fixture fixture = new Fixture();
        private readonly CompareLogic compareLogic;

        public WinLossViewModelTests(UserFixture userFixture)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
            this.userFixture = userFixture;
        }

        [Theory]
        [InlineData(4, "Master Win Loss")]
        [InlineData(3, "Agent Win Loss")]
        [InlineData(2, "Member Win Loss")]
        public void Title_WinLossLevel_ReturnCorrectTitle(
            byte custLevelId,
            string expectedTitle)
        {
            // Arrange
            var message = CreateWinlossQuery(custLevelId);
            var winlossViewModel = new WinLossViewModel(
                 Enumerable.Empty<WinLossModel>(),
                 fixture.Create<WinLossModel>(),
                 message,
                 fixture.Create<WinLossParam>(),
                 Enumerable.Empty<CustInfo>(),
                 Enumerable.Empty<GroupProductViewModel>(),
                 Enumerable.Empty<ProductDetailViewModel>(),
                 null);

            // Act & Assert
            Assert.Equal(winlossViewModel.Title, expectedTitle);
        }

        [Fact]
        public void WinLossViewModel_NullReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossModel();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = CreateWinLossViewModel(message);

            // Act
            var actualGrandTotal = winlossViewModel.GrandTotal;

            // Assert
            Assert.True(compareLogic.Compare(actualGrandTotal, expectedGrandTotal).AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultReportData()
        {
            // Arrange
            var expectedReportData = Enumerable.Empty<WinLossModel>();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                fixture.Create<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>(),
                null);

            // Act
            var actualReportData = winlossViewModel.ReportData;

            // Assert
            var result = compareLogic.Compare(actualReportData, expectedReportData);
            Assert.True(result.AreEqual);
        }

        [Fact]
        public void WinLossViewModel_EmptyReportData_ReturnsDefaultGrandTotal()
        {
            // Arrange
            var expectedGrandTotal = new WinLossModel();
            var message = CreateWinlossQuery(4);
            var winlossViewModel = CreateWinLossViewModel(message);

            // Act
            var actualGrandTotal = winlossViewModel.GrandTotal;

            // Assert
            Assert.True(compareLogic.Compare(actualGrandTotal, expectedGrandTotal).AreEqual);
        }

        [Fact]
        public void BuildDrillDownLink_ViewAgentReport_CallAddQueryStringParamsFuncToBuilBetListLink()
        {
            // Arrange
            SetUpForHelpers();

            var message = CreateWinlossQuery(2);
            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            message.IsHistoricReport = false;
            var winlossParam = new WinLossParam(1, 1, string.Empty, 1, fromDate, toDate, DateTime.MinValue, new List<int> { 1 }, new List<int> { 1 }, UserLevel.Agent, false, string.Empty, 0);
            var winlossViewModel = CreateWinLossViewModel(message, winlossParam);
            var winlossModel = new WinLossModel(1, "CustName", 0, 0, null, null, null, null, false);
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            var expectedRouteValues = new Dictionary<string, object>
            {
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate,
                ["ProductIds"] = "1",
                ["ResourceIds"] = "1",
                ["CustId"] = 1,
                ["CustName"] = "CustName"
            };

            // Act
            winlossViewModel.BuildDrillDownLink(winlossModel, "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(routeValue => compareLogic.Compare(expectedRouteValues, routeValue).AreEqual));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        public void BuildDrillDownLink_ViewingSuperOrMasterReport_CallReplaceOrAddQueryStringParamsFuncToBuildDrillDownLink(byte custLevelId)
        {
            // Arrange
            SetUpForHelpers();

            var message = CreateWinlossQuery(custLevelId);
            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var winlossParam = new WinLossParam(1, 1, string.Empty, 1, fromDate, toDate, DateTime.MinValue, new List<int> { 1 }, new List<int> { 1 }, UserLevel.Agent, false, string.Empty, 0);
            var winlossViewModel = new WinLossViewModel(
                            Enumerable.Empty<WinLossModel>(),
                            fixture.Create<WinLossModel>(),
                            message,
                            winlossParam,
                            Enumerable.Empty<CustInfo>(),
                            Enumerable.Empty<GroupProductViewModel>(),
                            Enumerable.Empty<ProductDetailViewModel>(),
                            null);
            var winlossModel = new WinLossModel(1, "CustName", 0, 0, null, null, null, null, false);
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            winlossViewModel.BuildDrillDownLink(winlossModel, "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "currentUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValue =>
                       (int)routeValue["CustId"] == 1
                       && (string)routeValue["CustName"] == "CustName"
                       && (byte)routeValue["CustLevelId"] == (custLevelId - 1)
                       && (string)routeValue["BreadcrumbLevelJson"] == "parsedJson"));
        }

        [Fact]
        public void SortByPropertyName_CanSort_ReturnsCorrectSortByPropertyName()
        {
            // Arrange
            var message = new WinLossQuery(userFixture.User) { SortPropertyName = "DownLineLevelName" };
            var viewModel = new WinLossViewModel(
                new List<WinLossModel> { fixture.Create<WinLossModel>(), fixture.Create<WinLossModel>() },
                fixture.Create<WinLossModel>(),
                message,
                fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>(),
                null);

            var expectedSortByPropertyName = "DownLineLevelName";

            // Act
            var actualSortByPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        private static void SetUpForHelpers()
        {
            Site.Helpers.JsonHelper.ToJsonFunc = Substitute.For<Func<object, string>>();
            Site.Helpers.JsonHelper.ToJsonFunc.Invoke(Arg.Any<object>()).Returns("parsedJson");
            Site.Helpers.UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapBetListPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapExMainPath(Arg.Any<string>()).Returns("BetListPath");
            Site.Helpers.UrlHelper.MapBetListPath(Arg.Any<string>()).Returns("NewBetListPath");
        }

        private WinLossQuery CreateWinlossQuery(byte custLevelId)
        {
            var winLossQuery = new WinLossQuery(userFixture.User);
            winLossQuery.CustLevelId = custLevelId;
            winLossQuery.FromDate = DefaultVariablesTest.CurrentDate;
            winLossQuery.ToDate = DefaultVariablesTest.CurrentDate;

            return winLossQuery;
        }

        private WinLossViewModel CreateWinLossViewModel(WinLossQuery message, WinLossParam winLossParam = null)
            => new WinLossViewModel(
                Enumerable.Empty<WinLossModel>(),
                fixture.Create<WinLossModel>(),
                message,
                winLossParam ?? fixture.Create<WinLossParam>(),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<GroupProductViewModel>(),
                Enumerable.Empty<ProductDetailViewModel>(),
                null);
    }
}