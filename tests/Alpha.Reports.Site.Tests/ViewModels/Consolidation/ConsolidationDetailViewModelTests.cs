namespace Alpha.Reports.Site.Tests.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fixtures;
    using NSubstitute;
    using Params;
    using Reports.Models;
    using Reports.Models.WinLossDetail;
    using Site.Requests.Consolidation;
    using Site.Requests.WinLossDetail;
    using Site.ViewModels.Consolidation;
    using Site.ViewModels.WinLossDetail;
    using Xunit;

    [Trait("Category", "Consolidation")]
    public class ConsolidationDetailViewModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly Func<string, IDictionary<string, object>, string> replaceOrAddQueryStringParamsFunc;

        public ConsolidationDetailViewModelTests()
        {
            anyValueCreatorFixture = new AnyValueCreatorFixture();

            Site.Helpers.UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapExMainPath(Arg.Any<string>()).Returns("BetListPath");

            replaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = replaceOrAddQueryStringParamsFunc;
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = replaceOrAddQueryStringParamsFunc;
        }

        [Fact]
        public void GetDownlineLinkRouteValues_Always_ReturnsRouteValue()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)4);

            var consolidationDetailViewModel = CreateConsolidationDetailViewModel(user);
            var winLossDetailModel = anyValueCreatorFixture.Create<WinLossDetailModel>();

            // Act
            consolidationDetailViewModel.BuildDrillDownLink(winLossDetailModel, "currentURL");

            // Assert
            replaceOrAddQueryStringParamsFunc.Received().Invoke("currentURL", Arg.Any<Dictionary<string, object>>());
        }

        [Fact]
        public void GetBetListLinkRouteValues_Always_ReturnsRouteValue()
        {
            // Arrange
            var user = Substitute.For<IUser>();
            user.LevelId.Returns((byte)2);

            var consolidationDetailViewModel = CreateConsolidationDetailViewModel(user);
            var winLossDetailModel = anyValueCreatorFixture.Create<WinLossDetailModel>();

            // Act
            consolidationDetailViewModel.BuildDrillDownLink(winLossDetailModel, "currentURL");

            // Assert
            replaceOrAddQueryStringParamsFunc.Received().Invoke("BetListPath", Arg.Any<Dictionary<string, object>>());
        }

        private ConsolidationDetailViewModel CreateConsolidationDetailViewModel(IUser user)
        {
            var query = new ConsolidationDetailQuery(user);
            var winLossDetailViewModel = new WinLossDetailViewModel(
                new WinLossDetailQuery(user),
                anyValueCreatorFixture.Create<WinLossDetailParam>(),
                anyValueCreatorFixture.Create<PagedReportData<WinLossDetailModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<CustInfo>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>());

            var consolidationDetailViewModel = new ConsolidationDetailViewModel(
                query,
                null,
                anyValueCreatorFixture.Create<ConsolidationFilterViewModel>(),
                winLossDetailViewModel);

            return consolidationDetailViewModel;
        }
    }
}
