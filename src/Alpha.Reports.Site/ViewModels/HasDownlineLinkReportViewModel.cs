namespace Alpha.Reports.Site.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using Reports.Models;

    public abstract class HasDownlineLinkReportViewModel<T> where T : class
    {
        private const string DefaultBetListPath = "~/_BetList/BetList.aspx";

        protected HasDownlineLinkReportViewModel(
            UserLevel custLevel,
            IEnumerable<CustInfo> breadcrumbLevel)
        {
            BetListPath = UrlHelper.MapExMainPath(DefaultBetListPath);
            CustLevel = custLevel;
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public UserLevel CustLevel { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        protected string BetListPath { get; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed!")]
        public virtual string BuildDrillDownLink(T model, string currentUrl)
        {
            if (CustLevel.IsAgentLevel())
            {
                return BuildBetListLink(model);
            }

            return BuildDownlineLink(model, currentUrl);
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed!")]
        protected virtual string BuildDownlineLink(T model, string currentUrl)
        {
            var routeValues = GetDownlineLinkRouteValues(model);
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentUrl,
                new Dictionary<string, object>(routeValues)
                {
                    { "CustLevelId", CustLevel.GetDirectDownLineLevel().Value },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }

        protected virtual string BuildBetListLink(T model)
        {
            return UrlHelper.AddQueryStringParamsFunc(
                BetListPath,
                GetBetListLinkRouteValues(model));
        }

        protected abstract IDictionary<string, object> GetDownlineLinkRouteValues(T model);

        protected abstract IDictionary<string, object> GetBetListLinkRouteValues(T model);
    }
}