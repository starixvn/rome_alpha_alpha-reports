namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Params.Consolidation;
    using Requests.Consolidation;

    public class ConsolidationReportByProductViewModel : ConsolidationReportViewModel<ConsolidationByProductViewModel>
    {
        public ConsolidationReportByProductViewModel(
            ConsolidationByProductQuery message,
            GetConsolidationParam param,
            IEnumerable<SelectListItem> products,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<ConsolidationByProductViewModel> reportData) :
            base(message, param, products, currencies, reportData)
        {
        }
    }
}