namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using Reports.Models;
    using Web.Helpers;

    public class BreadcrumbLevelViewModel
    {
        public BreadcrumbLevelViewModel(
            IEnumerable<CustInfo> breadcrumbLevel,
            string breadcrumbLevelJson = null)
        {
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed")]
        public string BuildBreadcrumbLink(CustInfo model, string currentUrl)
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentUrl,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", model.CustLevelId },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }

        public string BuildBreadcrumbLinkByLevel(CustInfo model, string currentQuery, Dictionary<int, string> breadcrumbActions)
        {
            var url = UrlHelper.GetSiteUrl(breadcrumbActions[model.CustLevelId], currentQuery);

            return BuildBreadcrumbLink(model, url);
        }
    }
}