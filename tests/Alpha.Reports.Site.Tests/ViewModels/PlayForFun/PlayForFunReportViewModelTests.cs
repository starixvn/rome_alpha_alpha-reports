namespace Alpha.Reports.Site.Tests.ViewModels.PlayForFun
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants;
    using Fixtures;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Site.Requests.PlayForFun;
    using Xunit;

    [Trait("Category", "PlayForFun")]
    [Collection("UserCollection")]
    public class PlayForFunReportViewModelTests
    {
        private readonly IUser user;
        private readonly AnyValueCreatorFixture anyValueCreatorFixture;

        public PlayForFunReportViewModelTests(UserFixture userFixture)
        {
            user = userFixture.User;
            anyValueCreatorFixture = new AnyValueCreatorFixture();
        }

        [Fact]
        public void GetBetListLinkRouteValues_Always_ThrowsNotImplementedException()
        {
            // Arrange
            var query = new PlayForFunQuery(user);
            var viewModel = new PlayForFunReportViewModelForTesting(
                query,
                anyValueCreatorFixture.Create<PlayForFunParam>(),
                anyValueCreatorFixture.Create<IEnumerable<CustInfo>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<ReportData<PlayForFunModel>>());

            var model = anyValueCreatorFixture.Create<PlayForFunModel>();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => viewModel.CallGetBetListLinkRouteValues(model));
        }

        [Fact]
        public void GetDownlineLinkRouteValues_Always_ReturnsCorrectRouteValues()
        {
            // Arrange
            var query = new PlayForFunQuery(user)
            {
                SortPropertyName = "Turnover",
                CustId = 1,
                CustName = "supertest1",
                UserSelectedProductId = 6,
                UserSelectedStatusId = 2,
                UserSelectedDateRangeOption = 7,
                FromDate = DefaultVariables.CurrentDate,
                ToDate = DefaultVariables.CurrentDate
            };

            var viewModel = new PlayForFunReportViewModelForTesting(
                query,
                anyValueCreatorFixture.Create<PlayForFunParam>(),
                anyValueCreatorFixture.Create<IEnumerable<CustInfo>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<IEnumerable<SelectListItem>>(),
                anyValueCreatorFixture.Create<ReportData<PlayForFunModel>>());

            var model = new PlayForFunModel(2, "master1", 1, 2, 3);
            var expectedRouteValues = new Dictionary<string, object>
            {
                ["IsFilterVisible"] = true,
                ["IsDescendingSort"] = false,
                ["SortPropertyName"] = null,
                ["IsDescendingSort"] = false,
                ["CustId"] = 2,
                ["CustName"] = "master1",
                ["UserSelectedProductId"] = 6,
                ["UserSelectedStatusId"] = 2,
                ["UserSelectedDateRangeOption"] = 7,
                ["FromDate"] = DefaultVariables.CurrentDate,
                ["ToDate"] = DefaultVariables.CurrentDate
            };

            // Act
            var actualRouteValues = viewModel.CallGetDownlineLinkRouteValues(model);

            // Assert
            Assert.Equal(expectedRouteValues, actualRouteValues);
        }
    }
}
