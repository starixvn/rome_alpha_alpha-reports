namespace Alpha.Reports.Site.Tests.ViewModels.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NSubstitute;
    using Reports.Models;
    using Site.Helpers;
    using Site.ViewModels.Components;
    using Xunit;

    public class BreadcrumbLevelViewModelTests
    {
        [Fact]
        public void BuildBreadcrumbLink_Always_CallReplaceOrAddQueryStringParamsFunc()
        {
            // Arrange
            var custInfo = new CustInfo(1, "CustName", 2);
            var viewModel = new BreadcrumbLevelsViewModel(Enumerable.Empty<CustInfo>());
            JsonHelper.ToJsonFunc = (data) => "BreadcrumbLevelsJson";
            UrlHelper.ReplaceOrAddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();

            // Act
            viewModel.BuildBreadcrumbLink(custInfo, "currentUrl");

            // Assert
            UrlHelper.ReplaceOrAddQueryStringParamsFunc
                .Received()
                .Invoke(
                "currentUrl",
                Arg.Is<Dictionary<string, object>>(
                    routeValues => (int)routeValues["CustId"] == custInfo.CustId
                                && (string)routeValues["CustName"] == custInfo.CustName
                                && (byte)routeValues["CustLevelId"] == custInfo.CustLevelId
                                && (string)routeValues["BreadcrumbLevelsJson"] == viewModel.BreadcrumbLevelsJson));
        }
    }
}