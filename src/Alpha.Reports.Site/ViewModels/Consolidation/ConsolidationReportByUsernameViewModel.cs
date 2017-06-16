namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Params.Consolidation;
    using Requests.Consolidation;

    public class ConsolidationReportByUsernameViewModel : ConsolidationReportViewModel<ConsolidationByUsernameViewModel>
    {
        public ConsolidationReportByUsernameViewModel(
             GetConsolidationQuery message,
             GetConsolidationParam param,
             IEnumerable<SelectListItem> products,
             IEnumerable<SelectListItem> currencies,
             IEnumerable<ConsolidationByUsernameViewModel> reportData) :
            base(message, param, products, currencies, reportData)
        {
        }
    }
}