namespace Alpha.Reports.Site.Tests.ViewModels.PlayForFun
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Site.Requests.PlayForFun;
    using Site.ViewModels.PlayForFun;

    public class PlayForFunReportViewModelForTesting : PlayForFunReportViewModel
    {
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2100:CodeLineMustNotBeLongerThan",
            Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public PlayForFunReportViewModelForTesting(PlayForFunQuery query, PlayForFunParam param, IEnumerable<CustInfo> breadcrumbLevel, IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> statuses, ReportData<PlayForFunModel> reportData)
            : base(query, param, breadcrumbLevel, products, statuses, reportData)
        {
        }

        public IDictionary<string, object> CallGetDownlineLinkRouteValues(PlayForFunModel model)
            => GetDownlineLinkRouteValues(model);

        public IDictionary<string, object> CallGetBetListLinkRouteValues(PlayForFunModel model)
            => GetBetListLinkRouteValues(model);
    }
}
