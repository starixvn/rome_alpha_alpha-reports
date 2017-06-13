namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using Reports.Models;

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
        public string BuildBreadcrumbLink(CustInfo model, string currentLink, Dictionary<int, string> breadcrumbParamByLevel)
        {
            var url = breadcrumbParamByLevel == null
                ? currentLink
                : UrlHelper.GetSiteUrl(breadcrumbParamByLevel[model.CustLevelId], currentLink);

            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                url,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", model.CustLevelId },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }
    }
}