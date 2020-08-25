namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Linq;

    public class ProductDetailMultiSelectionViewModel
    {
        public ProductDetailMultiSelectionViewModel(
            IEnumerable<ProductDetailViewModel> productDetails,
            string userSelectedResourceIds,
            string language)
        {
            ProductDetails = productDetails;
            UserSelectedResourceIds = userSelectedResourceIds;
            Language = language;
        }

        public IEnumerable<ProductDetailViewModel> ProductDetails { get; }

        public string UserSelectedResourceIds { get; }

        public string Language { get; }

        public bool HasData => ProductDetails?.Any() == true;
    }
}