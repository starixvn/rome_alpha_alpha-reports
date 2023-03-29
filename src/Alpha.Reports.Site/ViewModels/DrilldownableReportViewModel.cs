namespace Alpha.Reports.Site.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Helpers;
    using Reports.Models;

    public abstract class DrillDownableReportViewModel<T> where T : class
    {
        private const string DefaultBetListPath = "~/_BetList/BetList.aspx";

        protected DrillDownableReportViewModel(
            UserLevel custLevel,
            IEnumerable<CustInfo> breadcrumbLevel,
            string defaultBetListPath = DefaultBetListPath)
        {
            BetListPath = defaultBetListPath;
            CustLevel = custLevel;
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public UserLevel CustLevel { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        protected string BetListPath { get; set; }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed!")]
        public virtual string BuildDrillDownLink(T model, string currentLink)
            => CustLevel.IsAgentLevel()
            ? BuildBetListLink(model)
            : BuildDownlineLink(model, currentLink);

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Reviewed!")]
        protected virtual string BuildDownlineLink(T model, string currentLink)
        {
            var routeValues = GetDownlineLinkRouteValues(model);
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentLink,
                new Dictionary<string, object>(routeValues)
                {
                    { "CustLevelId", CustLevel.GetDirectDownLineLevel().Value },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }

        protected virtual string BuildBetListLink(T model)
            => UrlHelper.AddQueryStringParamsFunc(
                BetListPath,
                GetBetListLinkRouteValues(model));

        protected abstract IDictionary<string, object> GetDownlineLinkRouteValues(T model);

        protected abstract IDictionary<string, object> GetBetListLinkRouteValues(T model);

        public string GetFirstLastName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                return $"{firstName}/ {lastName}";
            }

            return $"{firstName}{lastName}";
        }
    }
}