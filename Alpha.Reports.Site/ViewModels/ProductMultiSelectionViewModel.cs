namespace Alpha.Reports.Site.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Newtonsoft.Json;

    public class ProductMultiSelectionViewModel
    {
        public ProductMultiSelectionViewModel(
            IEnumerable<Product> userProducts,
            IEnumerable<int> userSelectedProductIds,
            string language)
        {
            UserProducts = userProducts;
            UserSelectedProductIds = userSelectedProductIds.ToList();
            Language = language;
        }

        public IEnumerable<Product> UserProducts { get; }

        public string JsonUserProducts => JsonConvert.SerializeObject(UserProducts);

        public List<int> UserSelectedProductIds { get; }

        public string Language { get; }
    }
}