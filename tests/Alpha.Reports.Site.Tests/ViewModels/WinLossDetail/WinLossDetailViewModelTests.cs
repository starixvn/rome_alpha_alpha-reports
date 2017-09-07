namespace Alpha.Reports.Site.Tests.ViewModels.WinLossDetail
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Constants;
    using Enumerations;
    using Fixtures;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Requests.WinLoss;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "WinLossDetail")]
    [Collection("UserCollection")]
    public class WinLossDetailViewModelTests
    {
        private const int AllBetProductId = 21;
        private readonly UserFixture userFixture;
        private readonly CompareLogic compareLogic;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public WinLossDetailViewModelTests(UserFixture userFixture)
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
            var message = new WinLossDetailQuery(userFixture.User);
            message.IsHistoricReport = false;
            message.CustLevelId = 2; // Agent Level
            var winLossDetailViewModel = CreateWinLossDetailViewModel(fromDate, toDate, message);
            var winlossDetailModel = new WinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            var expectedRouteValues = new Dictionary<string, object>
            {
                ["FromDate"] = fromDate,
                ["ToDate"] = toDate,
                ["ProductIds"] = "1",
                ["CustId"] = 1,
                ["CustName"] = "CustName"
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
            var message = new WinLossDetailQuery(userFixture.User);
            message.CustLevelId = custLevelId; // Agent Level
            var winLossDetailViewModel = CreateWinLossDetailViewModel(fromDate, toDate, message);
            var winlossDetailModel = new WinLossDetailModel(1, "CustName", 0, 0, 0, 0, null, null, null, null);
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
            var viewModel = new WinLossDetailViewModel(
                new WinLossDetailQuery(user),
                anyValueCreatorFixture.Create<WinLossDetailParam>(),
                new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, null),
                null,
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
            var viewModel = new WinLossDetailViewModel(
                new WinLossDetailQuery(user),
                anyValueCreatorFixture.Create<WinLossDetailParam>(),
                CreatePagedReportData(),
                null,
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
            var viewModel = new WinLossDetailViewModel(
                new WinLossDetailQuery(user)
                {
                    SortPropertyName = notInRangeSortByPropertyName
                },
                anyValueCreatorFixture.Create<WinLossDetailParam>(),
                CreatePagedReportData(),
                null,
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
            var viewModel = new WinLossDetailViewModel(
                new WinLossDetailQuery(user)
                {
                    SortPropertyName = inRangeSortByPropertyName
                },
                anyValueCreatorFixture.Create<WinLossDetailParam>(),
                CreatePagedReportData(),
                null,
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
            var message = new WinLossDetailQuery(userFixture.User)
            {
                SortPropertyName = sortPropertyName
            };

            var reportData = new List<WinLossDetailModel>
            {
                default(WinLossDetailModel),
                default(WinLossDetailModel)
            };

            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, sortPropertyName, false, false),
                new PagedReportData<WinLossDetailModel>(reportData, 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());
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
            var message = new WinLossDetailQuery(userFixture.User);

            var reportData = new List<WinLossDetailModel>
            {
                default(WinLossDetailModel),
                default(WinLossDetailModel)
            };

            var winLossDetailViewModel = new WinLossDetailViewModel(
                message,
                new WinLossDetailParam(0, 0, 0, DateTime.Now, DateTime.Now, Enumerable.Empty<int>(), message.CustLevel, "CustName", isDescendingSort, false),
                new PagedReportData<WinLossDetailModel>(reportData, 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                Enumerable.Empty<SelectListItem>());
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

        private static WinLossDetailViewModel CreateWinLossDetailViewModel(DateTime fromDate, DateTime toDate, WinLossDetailQuery message)
        {
            return Substitute.ForPartsOf<WinLossDetailViewModel>(
                message,
                new WinLossDetailParam(0, 0, 0, fromDate, toDate, new List<int> { 1 }, message.CustLevel, "CustName", false, false),
                new PagedReportData<WinLossDetailModel>(Enumerable.Empty<WinLossDetailModel>(), 0, 0, 0, default(WinLossDetailModel)),
                Enumerable.Empty<CustInfo>(),
                 new List<SelectListItem>
                 {
                     new SelectListItem { Value = AllBetProductId.ToString() }
                 });
        }

        private PagedReportData<WinLossDetailModel> CreatePagedReportData()
            => new PagedReportData<WinLossDetailModel>(
                new List<WinLossDetailModel>
                {
                    anyValueCreatorFixture.Create<WinLossDetailModel>(),
                    anyValueCreatorFixture.Create<WinLossDetailModel>()
                },
                0,
                0,
                0,
                null);
    }
}