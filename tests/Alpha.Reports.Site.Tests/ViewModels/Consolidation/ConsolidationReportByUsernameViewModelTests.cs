namespace Alpha.Reports.Site.Tests.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fixtures;
    using NSubstitute;
    using Params.Consolidation;
    using Reports.Models;
    using Site.Requests.Consolidation;
    using Site.ViewModels.Consolidation;
    using Xunit;

    [Trait("Category", "Consolidation")]
    [Collection("UserCollection")]
    public class ConsolidationReportByUsernameViewModelTests
    {
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;
        private readonly IUser user;

        public ConsolidationReportByUsernameViewModelTests(UserFixture userFixture)
        {
            anyValueCreatorFixture = new AnyValueCreatorFixture();
            user = userFixture.User;
        }

        [Fact]
        public void BuildDrillDownLink_DrilldownFromProduct_BuildsWithCorrectRouteValues()
        {
            // Arrange
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var viewModel = new ConsolidationReportByUsernameViewModel(
                new GetConsolidationQuery(user),
                anyValueCreatorFixture.Create<GetConsolidationParam>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<ConsolidationByUsernameViewModel>>());
            var drilldownFromDate = DateTime.MinValue;

            // Act
            viewModel.BuildDrillDownLink(
                "drilldownUrl",
                "drilldownFromType",
                "drilldownFromProductIds",
                drilldownFromDate,
                anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>());

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "drilldownUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (string)routeValues["DrilldownFromProductIds"] == "drilldownFromProductIds"
                    && (DateTime)routeValues["DrilldownDate"] == drilldownFromDate));
        }

        [Fact]
        public void BuildDrillDownLink_NotDrilldownFromProduct_BuildsWithCorrectRouteValues()
        {
            // Arrange
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var param = new GetConsolidationParam(1, 1, DateTime.MinValue, DateTime.MinValue, new List<int> { 1 });
            var viewModel = new ConsolidationReportByUsernameViewModel(
                new GetConsolidationQuery(user),
                param,
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<ConsolidationByUsernameViewModel>>());
            var drilldownFromDate = DateTime.MinValue;

            // Act
            viewModel.BuildDrillDownLink(
                "drilldownUrl",
                "drilldownFromType",
                null,
                drilldownFromDate,
                anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>());

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "drilldownUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (string)routeValues["DrilldownFromProductIds"] == "1"
                    && (DateTime)routeValues["DrilldownDate"] == drilldownFromDate));
        }
    }
}
