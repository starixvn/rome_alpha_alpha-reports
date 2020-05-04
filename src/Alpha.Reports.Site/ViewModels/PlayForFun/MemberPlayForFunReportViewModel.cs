namespace Alpha.Reports.Site.ViewModels.PlayForFun
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Params;
    using Reports.Models;
    using Reports.Models.PlayForFun;
    using Requests.PlayForFun;

    public class MemberPlayForFunReportViewModel : PlayForFunReportViewModel
    {
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public MemberPlayForFunReportViewModel(
            PlayForFunQuery query, 
            PlayForFunParam param, 
            IEnumerable<CustInfo> breadcrumbLevel, 
            IEnumerable<GroupProductViewModel> groupProducts, 
            IEnumerable<SelectListItem> statuses, 
            ReportData<PlayForFunModel> reportData)
            : base(query, param, breadcrumbLevel, groupProducts, statuses, reportData)
        {
            var isResetSort = reportData.DataSource == null
                || reportData.DataSource.Count() <= 1
                || string.IsNullOrWhiteSpace(query.SortPropertyName)
                || string.Equals(query.SortPropertyName, MasterOrAgentPlayForFunReportViewModel.SortByNumberOfPlayers);

            SortPropertyName = isResetSort ? SortByTurnover : query.SortPropertyName;
            IsDescendingSort = isResetSort || query.IsDescendingSort;
        }
    }
}