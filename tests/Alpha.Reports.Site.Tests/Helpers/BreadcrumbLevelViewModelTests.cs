namespace Alpha.Reports.Site.Tests.Helpers
{
    using System.Collections.Generic;
    using Reports.Models;
    using Site.Helpers;
    using Site.ViewModels.Components;
    using Xunit;

    public class BreadcrumbLevelViewModelTests
    {
        public BreadcrumbLevelViewModelTests()
        {
            UrlHelper.ReplaceOrAddQueryStringParamsFunc = (url, routeValues) => url;
            UrlHelper.GetSiteUrl = (levelId, query) => levelId + "?" + query;
        }

        [Fact]
        public void BuildBreadcrumbLinkByLevel_StateUnderTest_ExpectedBehaviour()
        {
            // Arrange
            var custInfo = new CustInfo(1, string.Empty, 1);
            var breadcrumbLevelViewModel = new BreadcrumbLevelViewModel(new List<CustInfo> { custInfo }, string.Empty);
            var query = "Query";
            var expectedLink = "1?Query";

            // Act
            var actualLink = breadcrumbLevelViewModel.BuildBreadcrumbLinkByLevel(custInfo, query);

            // Assert
            Assert.Equal(expectedLink, actualLink);
        }
    }
}
