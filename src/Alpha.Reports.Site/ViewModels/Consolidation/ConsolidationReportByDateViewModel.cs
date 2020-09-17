namespace Alpha.Reports.Site.ViewModels.Consolidation
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using Params.Consolidation;
    using Requests.Consolidation;

    public class ConsolidationReportByDateViewModel : ConsolidationReportViewModel<ConsolidationByDateViewModel>
    {
        public ConsolidationReportByDateViewModel(
            ConsolidationByDateQuery message,
            GetConsolidationParam param,
            IEnumerable<GroupProductViewModel> groupProducts,
            IEnumerable<ProductDetailViewModel> productDetails,
            IEnumerable<SelectListItem> currencies,
            IEnumerable<ConsolidationByDateViewModel> reportData)
            : base(message, param, groupProducts, productDetails, currencies, reportData)
        {
        }
    }
}