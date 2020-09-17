namespace Alpha.Reports.Site.ViewModels.Components
{
    using System.Web.Mvc;

    public class ProductDetailViewModel : SelectListItem
    {
        public ProductDetailViewModel(int productId, int resourceId, string resourceText, bool selected)
        {
            ProductId = productId;
            Value = resourceId.ToString();
            Text = resourceText;
            Selected = selected;
        }

        public int ProductId { get; }
    }
}