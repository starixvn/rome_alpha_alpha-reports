namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ExcludeProductViewModel
    {
        public ExcludeProductViewModel(
            IEnumerable<SelectListItem> excludeProducts,
            string userSelectedExcludeProductIds)
        {
            ExcludeProducts = excludeProducts;
            UserSelectedExcludeProductIds = userSelectedExcludeProductIds;
        }

        public IEnumerable<SelectListItem> ExcludeProducts { get; }

        public string UserSelectedExcludeProductIds { get; }
    }
}