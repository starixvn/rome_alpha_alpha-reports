namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Collections.Generic;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<GroupProductViewModel> groupProducts,
            string userSelectedProductIds,
            string language)
        {
            GroupProducts = groupProducts;
            UserSelectedProductIds = userSelectedProductIds;
            Language = language;
        }

        public IEnumerable<GroupProductViewModel> GroupProducts { get; }

        public string Language { get; }

        public string UserSelectedProductIds { get; }
    }
}