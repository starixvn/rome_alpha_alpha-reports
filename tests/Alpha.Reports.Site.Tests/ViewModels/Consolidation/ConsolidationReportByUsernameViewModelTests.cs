namespace Alpha.Reports.Site.Tests.ViewModels.Consolidation
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Constants;
    using Fixtures;
    using NSubstitute;
    using Params.Consolidation;
    using Reports.Models;
    using Site.Requests.Consolidation;
    using Site.ViewModels.Consolidation;
    using Utils;
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

        [Theory]
        [InlineData(null, "2016/12/2", null, "1")]
        [InlineData("2016/12/2", "2016/12/2", null, "1")]
        [InlineData("2016/12/3", "2016/12/3", "1,2", "1,2")]
        public void BuildDrillDownLink_MemberLevel_ReturnCorrectBetListLink(
            string drilldownDate,
            string expectedDate,
            string drilldownFromProductIds,
            string expectedProductIds)
        {
            // Arrange
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            Site.Helpers.UrlHelper.MapBetListPath = Substitute.For<Func<string, string>>();
            Site.Helpers.UrlHelper.MapBetListPath(Arg.Any<string>()).Returns("NewBetListPath");

            var member = Substitute.For<IUser>();
            member.LevelId.Returns((byte)1);
            member.Id.Returns(10);

            var query = new GetConsolidationQuery(member)
            {
                FromDate = DefaultVariablesTest.CurrentDate,
                ToDate = DefaultVariablesTest.CurrentDate
            };

            var param = new GetConsolidationParam(0, 0, DefaultVariablesTest.CurrentDate, DefaultVariablesTest.CurrentDate, new List<int> { 1 }, new List<int> { 1 }, string.Empty);
            var row = new ConsolidationByUsernameViewModel(1, 1, "Member", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty);

            var viewModel = new ConsolidationReportByUsernameViewModel(
                query,
                param,
                anyValueCreatorFixture.Create<IEnumerable<GroupProductViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<ProductDetailViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<ConsolidationByUsernameViewModel>>());
            var drilldownFromDate = drilldownDate == null ? (DateTime?)null : Convert.ToDateTime(drilldownDate);
            var expectedFromDate = Convert.ToDateTime(expectedDate);

            // Act
            viewModel.BuildDrillDownLink(
                "drilldownUrl",
                "drilldownFromType",
                drilldownFromProductIds,
                drilldownFromDate,
                row);

            // Assert
            Site.Helpers.UrlHelper.AddQueryStringParamsFunc.Received().Invoke(
                "NewBetListPath",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (DateTime)routeValues["FromDate"] == expectedFromDate
                    && (DateTime)routeValues["ToDate"] == expectedFromDate
                    && (string)routeValues["ProductIds"] == expectedProductIds
                    && (int)routeValues["FilterPostback"] == 1
                    && (int)routeValues["CustId"] == 1
                    && (string)routeValues["CustName"] == "Member"
                    && (bool)routeValues["IsGroupHeadline"] == true));
        }

        [Fact]
        public void BuildDrillDownLink_DrilldownFromProduct_BuildsWithCorrectRouteValues()
        {
            // Arrange
            SystemTime.Today = DefaultVariablesTest.CurrentDate;
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var param = new GetConsolidationParam(1, 1, DefaultVariablesTest.CurrentDate, DefaultVariablesTest.CurrentDate, new List<int> { 1, 2 }, new List<int> { 1, 2 }, string.Empty);

            var viewModel = new ConsolidationReportByUsernameViewModel(
                new GetConsolidationQuery(user),
                param,
                anyValueCreatorFixture.Create<IEnumerable<GroupProductViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<ProductDetailViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<ConsolidationByUsernameViewModel>>());
            DateTime? drilldownFromDate = null;
            var row = new ConsolidationByUsernameViewModel(1, 2, "Agent", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, string.Empty);

            // Act
            viewModel.BuildDrillDownLink(
                "drilldownUrl",
                "drilldownFromType",
                "drilldownFromProductIds",
                drilldownFromDate,
                row);

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "drilldownUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (string)routeValues["UserSelectedProductIds"] == "drilldownFromProductIds"
                    && (DateTime)routeValues["FromDate"] == DateTime.MinValue
                    && (DateTime)routeValues["ToDate"] == DefaultVariablesTest.CurrentDate
                    && (DateTime)routeValues["ConsolidationFromDate"] == DateTime.MinValue
                    && (DateTime)routeValues["ConsolidationToDate"] == DefaultVariablesTest.CurrentDate
                    && (string)routeValues["ConsolidationProductIds"] == "1,2"));
        }

        [Fact]
        public void BuildDrillDownLink_NotDrilldownFromProduct_BuildsWithCorrectRouteValues()
        {
            // Arrange
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
            var param = new GetConsolidationParam(1, 1, DateTime.MinValue, DateTime.MinValue, new List<int> { 1 }, new List<int> { 1 }, string.Empty);
            var viewModel = new ConsolidationReportByUsernameViewModel(
                new GetConsolidationQuery(user),
                param,
                anyValueCreatorFixture.Create<IEnumerable<GroupProductViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<ProductDetailViewModel>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<ConsolidationByUsernameViewModel>>());
            var drilldownFromDate = new DateTime(2017, 9, 1);

            // Act
            viewModel.BuildDrillDownLink(
                "drilldownUrl",
                "drilldownFromType",
                "1",
                drilldownFromDate,
                anyValueCreatorFixture.Create<ConsolidationByUsernameViewModel>());

            // Assert
            Site.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc.Received().Invoke(
                "drilldownUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValues =>
                    (string)routeValues["UserSelectedProductIds"] == "1"
                    && (DateTime)routeValues["FromDate"] == drilldownFromDate
                    && (DateTime)routeValues["ToDate"] == drilldownFromDate));
        }
    }
}
