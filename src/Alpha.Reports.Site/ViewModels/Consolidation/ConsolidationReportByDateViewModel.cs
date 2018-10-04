namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Params.Consolidation;
    using Reports.Models;
    using Requests.Consolidation;

    public class ConsolidationReportByDateViewModel : ConsolidationReportViewModel<ConsolidationByDateViewModel>
    {
        public ConsolidationReportByDateViewModel(
            ConsolidationByDateQuery message,
            GetConsolidationParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> excludeProducts,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<ConsolidationByDateViewModel> reportData)
            : base(message, param, products, excludeProducts, currencies, reportData)
        {
        }
    }
}