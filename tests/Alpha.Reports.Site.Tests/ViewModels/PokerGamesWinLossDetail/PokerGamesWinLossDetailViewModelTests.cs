namespace Alpha.Reports.Site.Tests.ViewModels.PokerGamesWinLossDetail
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
    using Reports.Models;
    using Reports.Models.PokerGamesWinLossDetail;
    using Site.Requests.PokerGamesWinLossDetail;
    using Site.ViewModels.PokerGamesWinLossDetail;
    using Xunit;

    [Trait("Category", "PokerGamesWinLossDetail")]
    [Collection("UserCollection")]
    public class PokerGamesWinLossDetailViewModelTests
    {
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public PokerGamesWinLossDetailViewModelTests(UserFixture userFixture)
        {
            this.userFixture = userFixture;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageCode.English);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            compareLogic = new CompareLogic();
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void BuildDrillDownLink_ViewingAgentReport_CallAddQueryStringParamsFuncToBuildBetListLink()
        {
            // Arrange
            SetUpForHelpers();

            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var message = new PokerGamesWinLossDetailQuery(userFixture.User);
            message.IsHistoricReport = false;
            message.CustLevelId = 2; // Agent Level
            var winLossDetailViewModel = CreateWinLossDetailViewModel(fromDate, toDate, message);
            var winlossDetailModel = new PokerGamesWinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            var expectedRouteValues = new Dictionary<string, object>
            {
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate,
                ["ProductIds"] = "33",
                ["CustId"] = 1,
                ["CustName"] = "CustName",
                ["IsGroupHeadline"] = false
            };

            // Act
            winLossDetailViewModel.BuildDrillDownLink(winlossDetailModel, "currentUrl");

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

            var fromDate = new DateTime(1989, 5, 28);
            var toDate = new DateTime(1989, 5, 29);
            var message = new PokerGamesWinLossDetailQuery(userFixture.User);
            message.CustLevelId = custLevelId; // Agent Level
            var winLossDetailViewModel = CreateWinLossDetailViewModel(fromDate, toDate, message);
            var winlossDetailModel = new PokerGamesWinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            winLossDetailViewModel.BuildDrillDownLink(winlossDetailModel, "currentUrl");

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "currentUrl",
                 Arg.Is<Dictionary<string, object>>(
                     routeValues =>
                     (int)routeValues["CustId"] == 1
                     && (string)routeValues["CustName"] == "CustName"
                     && (byte)routeValues["CustLevelId"] == custLevelId - 1
                     && (string)routeValues["BreadcrumbLevelJson"] == "parsedJson"));
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void SortByPropertyName_CannotSort_ReturnsCustName(byte custLevelId)
        {
            // Arrange
            var expectedSortByPropertyName = "CustName";

            var user = Substitute.For<IUser>();
            user.LevelId.Returns(custLevelId);
            var viewModel = new PokerGamesWinLossDetailViewModel(
                new PokerGamesWinLossDetailQuery(user),
                anyValueCreatorFixture.Create<PokerGamesWinLossDetailParam>(),
                new PagedReportData<PokerGamesWinLossDetailModel>(Enumerable.Empty<PokerGamesWinLossDetailModel>(), 0, 0, 0, null),
                null);

            // Act
            var actualSortByPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(3)]
        [InlineData(2)]
        public void SortByPropertyName_NotSupportSortByPropertyName_ReturnsCustName(byte custLevelId)
        {
            // Arrange
            var expectedSortByPropertyName = "CustName";

            var user = Substitute.For<IUser>();
            user.LevelId.Returns(custLevelId);
            var viewModel = new PokerGamesWinLossDetailViewModel(
                new PokerGamesWinLossDetailQuery(user),
                anyValueCreatorFixture.Create<PokerGamesWinLossDetailParam>(),
                CreatePagedReportData(),
                null);

            // Act
            var actualSortByPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Theory]
        [InlineData(3, "SuperNetWinLoss.WinLoss")]
        [InlineData(3, "SuperNetWinLoss.Comm")]
        [InlineData(3, "SuperNetWinLoss.Total")]
        [InlineData(2, "SuperNetWinLoss.WinLoss")]
        [InlineData(2, "SuperNetWinLoss.Comm")]
        [InlineData(2, "SuperNetWinLoss.Total")]
        [InlineData(2, "MasterNetWinLoss.WinLoss")]
        [InlineData(2, "MasterNetWinLoss.Comm")]
        [InlineData(2, "MasterNetWinLoss.Total")]
        public void SortByPropertyName_SupportedSortByPropertyNameNotInRange_ReturnsCustName(
            byte custLevelId,
            string notInRangeSortByPropertyName)
        {
            // Arrange
            var expectedSortByPropertyName = "CustName";
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(custLevelId);
            var viewModel = new PokerGamesWinLossDetailViewModel(
                new PokerGamesWinLossDetailQuery(user)
                {
                    SortPropertyName = notInRangeSortByPropertyName
                },
                anyValueCreatorFixture.Create<PokerGamesWinLossDetailParam>(),
                CreatePagedReportData(),
                null);

            // Act
            var actualSortByPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Theory]
        [InlineData(4, "SuperNetWinLoss.WinLoss")]
        [InlineData(4, "SuperNetWinLoss.Comm")]
        [InlineData(4, "SuperNetWinLoss.Total")]
        [InlineData(4, "MasterNetWinLoss.WinLoss")]
        [InlineData(4, "MasterNetWinLoss.Comm")]
        [InlineData(4, "MasterNetWinLoss.Total")]
        [InlineData(4, "AgentNetWinLoss.WinLoss")]
        [InlineData(4, "AgentNetWinLoss.Comm")]
        [InlineData(4, "AgentNetWinLoss.Total")]
        [InlineData(4, "CustId")]
        [InlineData(4, "CustName")]
        [InlineData(4, "Turnover")]
        [InlineData(4, "NetTurnover")]
        [InlineData(4, "GrossComm")]
        [InlineData(4, "BetCount")]
        [InlineData(4, "Company")]
        [InlineData(3, "MasterNetWinLoss.WinLoss")]
        [InlineData(3, "MasterNetWinLoss.Comm")]
        [InlineData(3, "MasterNetWinLoss.Total")]
        [InlineData(3, "AgentNetWinLoss.WinLoss")]
        [InlineData(3, "AgentNetWinLoss.Comm")]
        [InlineData(3, "AgentNetWinLoss.Total")]
        [InlineData(3, "CustId")]
        [InlineData(3, "CustName")]
        [InlineData(3, "Turnover")]
        [InlineData(3, "NetTurnover")]
        [InlineData(3, "GrossComm")]
        [InlineData(3, "BetCount")]
        [InlineData(3, "Company")]
        [InlineData(2, "AgentNetWinLoss.WinLoss")]
        [InlineData(2, "AgentNetWinLoss.Comm")]
        [InlineData(2, "AgentNetWinLoss.Total")]
        [InlineData(2, "CustId")]
        [InlineData(2, "CustName")]
        [InlineData(2, "Turnover")]
        [InlineData(2, "NetTurnover")]
        [InlineData(2, "GrossComm")]
        [InlineData(2, "BetCount")]
        [InlineData(2, "Company")]
        public void SortByPropertyName_SupportedSortByPropertyNameInRange_ReturnsSupportedSortByPropertyName(
            byte custLevelId,
            string inRangeSortByPropertyName)
        {
            // Arrange
            var expectedSortByPropertyName = inRangeSortByPropertyName;
            var user = Substitute.For<IUser>();
            user.LevelId.Returns(custLevelId);
            var viewModel = new PokerGamesWinLossDetailViewModel(
                new PokerGamesWinLossDetailQuery(user)
                {
                    SortPropertyName = inRangeSortByPropertyName
                },
                anyValueCreatorFixture.Create<PokerGamesWinLossDetailParam>(),
                CreatePagedReportData(),
                null);

            // Act
            var actualSortByPropertyName = viewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortByPropertyName, actualSortByPropertyName);
        }

        [Fact]
        public void SortPropertyName_CanSort_ReturnsSortByPropertyNameFromParam()
        {
            // Arrange
            var sortPropertyName = "Turnover";
            var message = new PokerGamesWinLossDetailQuery(userFixture.User)
            {
                SortPropertyName = sortPropertyName
            };

            var reportData = new List<PokerGamesWinLossDetailModel>
            {
                default(PokerGamesWinLossDetailModel),
                default(PokerGamesWinLossDetailModel)
            };

            var winLossDetailViewModel = new PokerGamesWinLossDetailViewModel(
                message,
                new PokerGamesWinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.MinValue, message.CustLevel, sortPropertyName, false, false),
                new PagedReportData<PokerGamesWinLossDetailModel>(reportData, 0, 0, 0, default(PokerGamesWinLossDetailModel)),
                Enumerable.Empty<CustInfo>());
            var expectedSortPropertyName = sortPropertyName;

            // Act
            var actualSortPropertyName = winLossDetailViewModel.SortPropertyName;

            // Assert
            Assert.Equal(expectedSortPropertyName, actualSortPropertyName);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsDescendingSort_CanSort_ReturnsIsDescendingSortFromParam(bool isDescendingSort)
        {
            // Arrange
            var message = new PokerGamesWinLossDetailQuery(userFixture.User);

            var reportData = new List<PokerGamesWinLossDetailModel>
            {
                default(PokerGamesWinLossDetailModel),
                default(PokerGamesWinLossDetailModel)
            };

            var winLossDetailViewModel = new PokerGamesWinLossDetailViewModel(
                message,
                new PokerGamesWinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, DateTime.MinValue, message.CustLevel, "CustName", isDescendingSort, false),
                new PagedReportData<PokerGamesWinLossDetailModel>(reportData, 0, 0, 0, default(PokerGamesWinLossDetailModel)),
                Enumerable.Empty<CustInfo>());
            var expectedIsDescendingSort = isDescendingSort;

            // Act
            var actualIsDescendingSort = winLossDetailViewModel.IsDescendingSort;

            // Assert
            Assert.Equal(expectedIsDescendingSort, actualIsDescendingSort);
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

        private static PokerGamesWinLossDetailViewModel CreateWinLossDetailViewModel(DateTime fromDate, DateTime toDate, PokerGamesWinLossDetailQuery message)
        {
            return Substitute.ForPartsOf<PokerGamesWinLossDetailViewModel>(
                message,
                new PokerGamesWinLossDetailParam(0, 0, 0, fromDate, toDate, DateTime.MinValue, message.CustLevel, "CustName", false, false),
                new PagedReportData<PokerGamesWinLossDetailModel>(Enumerable.Empty<PokerGamesWinLossDetailModel>(), 0, 0, 0, default(PokerGamesWinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                false);
        }

        private PagedReportData<PokerGamesWinLossDetailModel> CreatePagedReportData()
            => new PagedReportData<PokerGamesWinLossDetailModel>(
                new List<PokerGamesWinLossDetailModel>
                {
                    anyValueCreatorFixture.Create<PokerGamesWinLossDetailModel>(),
                    anyValueCreatorFixture.Create<PokerGamesWinLossDetailModel>()
                },
                0,
                0,
                0,
                null);
    }
}