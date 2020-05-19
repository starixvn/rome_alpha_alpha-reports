namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Params.Consolidation;
    using Requests.Consolidation;

    public class ConsolidationReportByUsernameViewModel : ConsolidationReportViewModel<ConsolidationByUsernameViewModel>
    {
        public ConsolidationReportByUsernameViewModel(
             GetConsolidationQuery message,
             GetConsolidationParam param,
             IEnumerable<GroupProductViewModel> groupProducts,
             IEnumerable<SelectListItem> currencies,
             IEnumerable<ConsolidationByUsernameViewModel> reportData)
            : base(message, param, groupProducts, currencies, reportData)
        {
        }
    }
}