namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<SelectListItem> products,
            string language)
        {
            Products = products;
            Language = language;
        }

        public IEnumerable<SelectListItem> Products { get; }

        public string Language { get; }
    }
}