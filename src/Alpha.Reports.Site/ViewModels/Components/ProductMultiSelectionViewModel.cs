namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<SelectListItem> products,
            string userSelectedProductIds,
            string language)
        {
            Products = products;
            UserSelectedProductIds = userSelectedProductIds;
            Language = language;
        }

        public IEnumerable<SelectListItem> Products { get; }

        public string Language { get; }

        public string UserSelectedProductIds { get; }
    }
}