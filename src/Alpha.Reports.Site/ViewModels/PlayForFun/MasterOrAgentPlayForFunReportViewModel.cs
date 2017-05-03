namespace Alpha.Reports.Site.ViewModels.PlayForFun
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;

    public class MasterOrAgentPlayForFunReportViewModel : PlayForFunReportViewModel
    {
        public const string SortByNumberOfPlayers = nameof(MasterAgentPlayForFunModel.NumberOfPlayers);

        [SuppressMessage(
              "StyleCop.CSharp.StylecopPlus.Rules",
              "SP2100:CodeLineMustNotBeLongerThan",
              Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public MasterOrAgentPlayForFunReportViewModel(
            PlayForFunQuery query, 
            PlayForFunParam param, 
            IEnumerable<CustInfo> breadcrumbLevel, 
            IEnumerable<SelectListItem> products, 
            IEnumerable<SelectListItem> statuses, 
            ReportData<PlayForFunModel> reportData)
            : base(query, param, breadcrumbLevel, products, statuses, reportData)
        {
            var isResetSort = reportData.DataSource == null
                || reportData.DataSource.Count() <= 1
                || string.IsNullOrWhiteSpace(query.SortPropertyName)
                || string.Equals(query.SortPropertyName, MemberPlayForFunViewModel.SortByStatusName);

            SortPropertyName = isResetSort ? SortByNumberOfPlayers : query.SortPropertyName;
            IsDescendingSort = isResetSort || query.IsDescendingSort;
            DrilldownAction = query.CustLevel.IsSuperLevel() ? "MasterOrAgent" : "Member";
        }

        public string DrilldownAction { get; }
    }
}