namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using Reports.Models;

    public class BreadcrumbLevelsViewModel
    {
        public BreadcrumbLevelsViewModel(
            IEnumerable<CustInfo> breadcrumbLevels,
            string breadcrumbLevelsJson = null)
        {
            BreadcrumbLevels = breadcrumbLevels;
            BreadcrumbLevelsJson = breadcrumbLevelsJson ?? JsonHelper.ToJsonFunc(breadcrumbLevels);
        }

        public IEnumerable<CustInfo> BreadcrumbLevels { get; }

        public string BreadcrumbLevelsJson { get; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed")]
        public string BuildBreadcrumbLink(
            CustInfo model,
            string currentUrl)
        {
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentUrl,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", model.CustLevelId },
                    { "BreadcrumbLevelsJson", BreadcrumbLevelsJson }
                });
        }
    }
}