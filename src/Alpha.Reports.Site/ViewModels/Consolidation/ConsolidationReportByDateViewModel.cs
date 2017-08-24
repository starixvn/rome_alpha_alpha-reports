namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Params.Consolidation;
    using Requests.Consolidation;

    public class ConsolidationReportByDateViewModel : ConsolidationReportViewModel<ConsolidationByDateViewModel>
    {
        public ConsolidationReportByDateViewModel(
            ConsolidationByDateQuery message,
            GetConsolidationParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<ConsolidationByDateViewModel> reportData) :
            base(message, param, products, currencies, reportData)
        {
        }
    }
}